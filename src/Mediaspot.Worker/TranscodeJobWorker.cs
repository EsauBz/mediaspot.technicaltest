using Mediaspot.Application.Common;
using Mediaspot.Application.Common.Queues;
using Mediaspot.Application.Transcoding.Commands.Complete;
using Mediaspot.Application.Transcoding.Commands.Fail;
using Mediaspot.Application.Transcoding.Commands.Start;
using Mediaspot.Domain.Transcoding;
using MediatR;

namespace Mediaspot.Worker;

/// <summary>
/// /// This is a dummy worker that simulates the processing of transcode jobs.
/// It retrieves pending jobs from the queue, processes them, and updates their status.
/// This is just a simulation to demonstrate how the worker would operate using a queue.
/// </summary>
/// <param name="serviceProvider"></param>
/// <param name="queue"></param>
/// <param name="logger"></param>
public class TranscodeJobWorker(
    IServiceProvider serviceProvider,
    ITranscodeQueue queue,
    ILogger<TranscodeJobWorker> logger,
    ITaskDelayer taskDelayer)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Transcode Job Worker was initiated and its hearing for new messages on queue.");

        // Waiting for new messages in the queue
        while (!stoppingToken.IsCancellationRequested)
        {
            var request = await queue.DequeueAsync(stoppingToken);
            logger.LogInformation("New Job received in queue for Asset: {AssetId}", request.AssetId);

            using var scope = serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var jobRepo = scope.ServiceProvider.GetRequiredService<ITranscodeJobRepository>();
            var sender = scope.ServiceProvider.GetRequiredService<ISender>();

            // 2. Worker creates a new TranscodeJob instance in DB
            var job = new TranscodeJob(request.AssetId, request.MediaFileId, request.Preset);
            await jobRepo.AddAsync(job, stoppingToken);
            await uow.SaveChangesAsync(stoppingToken);
            logger.LogInformation("Job {JobId} created in Pending status.", job.Id);
            try
            {
                await sender.Send(new StartTranscodeJobCommand(job.Id), stoppingToken);
                logger.LogInformation("Job {JobId} marked as Running.", job.Id);

                await taskDelayer.Delay(TimeSpan.FromSeconds(10), stoppingToken);

                await sender.Send(new CompleteTranscodeJobCommand(job.Id), stoppingToken);
                logger.LogInformation("Job: {JobId} mark as Succeeded.", job.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing Job {JobId}", job.Id);
                await sender.Send(new FailTranscodeJobCommand(job.Id, ex.Message), stoppingToken);
            }
        }
    }
}
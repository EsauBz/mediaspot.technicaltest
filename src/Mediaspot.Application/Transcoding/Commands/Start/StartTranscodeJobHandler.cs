// El handler con la lógica de negocio
using Mediaspot.Application.Common;
using MediatR;

namespace Mediaspot.Application.Transcoding.Commands.Start;
/// <summary>
/// Handler for start a transcode job task.
/// </summary>
/// <param name="jobRepository"></param>
/// <param name="unitOfWork"></param>
public sealed class StartTranscodeJobHandler(ITranscodeJobRepository jobRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<StartTranscodeJobCommand>
{
    public async Task Handle(StartTranscodeJobCommand request, CancellationToken ct)
    {
        // 1. Using the created get async method, we retrieve the TranscodeJob by its ID.
        var job = await jobRepository.GetAsync(request.JobId, ct)
                  ?? throw new KeyNotFoundException("TranscodeJob not found");

        // 2. Call the Domain method to mark the job as running.
        job.MarkRunning();

        // 3. We save the changes to the database.
        await unitOfWork.SaveChangesAsync(ct);
    }
}
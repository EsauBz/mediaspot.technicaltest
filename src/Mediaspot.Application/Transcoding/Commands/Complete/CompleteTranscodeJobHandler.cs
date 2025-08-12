using Mediaspot.Application.Common;
using MediatR;

namespace Mediaspot.Application.Transcoding.Commands.Complete
{
    /// <summary>
    /// Handler for the task complete a transcode job.
    /// </summary>
    /// <param name="jobRepository">Repository for transcode jobs.</param>
    /// <param name="unitOfWork">Unit of work for saving changes.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    public sealed class CompleteTranscodeJobHandler(ITranscodeJobRepository jobRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<CompleteTranscodeJobCommand>
    {
        public async Task Handle(CompleteTranscodeJobCommand request, CancellationToken ct)
        {
            // 1. Using the created get async method, we retrieve the TranscodeJob by its ID.
            var job = await jobRepository.GetAsync(request.JobId, ct)
                      ?? throw new KeyNotFoundException("TranscodeJob not found");

            // 2. Call the Domain method to mark the job as succeeded.
            job.MarkSucceeded();

            // 3. Save changes
            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}
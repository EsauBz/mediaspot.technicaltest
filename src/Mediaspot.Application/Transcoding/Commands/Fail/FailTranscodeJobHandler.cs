using MediatR;
using Mediaspot.Application.Common;

namespace Mediaspot.Application.Transcoding.Commands.Fail;

/// <summary>
/// The handler for the failing task in a transcode job.
/// </summary>
/// <param name="jobRepository"></param>
/// <param name="unitOfWork"></param>
public sealed class FailTranscodeJobHandler(ITranscodeJobRepository jobRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<FailTranscodeJobCommand>
{
    public async Task Handle(FailTranscodeJobCommand request, CancellationToken ct)
    {
        //1. Using the created get async method, we retrieve the TranscodeJob by its ID.
        var job = await jobRepository.GetAsync(request.JobId, ct)
                  ?? throw new KeyNotFoundException("TranscodeJob not found");

        // 2. We use the Domain method to mark the job as failed.
        job.MarkFailed();

        // 3. Save changes
        await unitOfWork.SaveChangesAsync(ct);
    }
}
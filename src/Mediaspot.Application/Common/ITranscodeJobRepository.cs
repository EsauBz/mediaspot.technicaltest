using Mediaspot.Domain.Transcoding;

namespace Mediaspot.Application.Common;

public interface ITranscodeJobRepository
{
    Task AddAsync(TranscodeJob job, CancellationToken ct);
    Task<bool> HasActiveJobsAsync(Guid assetId, CancellationToken ct);
    /// <summary>
    /// Retrieves a transcode job by its identifier.
    /// If the job does not exist, returns null.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns>TranscodeJob</returns>
    Task<TranscodeJob?> GetAsync(Guid id, CancellationToken ct);
}

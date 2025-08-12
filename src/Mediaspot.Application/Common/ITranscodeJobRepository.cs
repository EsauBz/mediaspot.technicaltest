using Mediaspot.Domain.Transcoding;

namespace Mediaspot.Application.Common;

public interface ITranscodeJobRepository
{
    Task AddAsync(TranscodeJob job, CancellationToken ct);
    Task<bool> HasActiveJobsAsync(Guid assetId, CancellationToken ct);
    Task<TranscodeJob?> GetAsync(Guid id, CancellationToken ct);
}

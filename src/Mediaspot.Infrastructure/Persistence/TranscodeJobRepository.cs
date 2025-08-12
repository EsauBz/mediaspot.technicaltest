using Mediaspot.Application.Common;
using Mediaspot.Domain.Transcoding;
using Microsoft.EntityFrameworkCore;

namespace Mediaspot.Infrastructure.Persistence;

public sealed class TranscodeJobRepository(MediaspotDbContext db) : ITranscodeJobRepository
{
    public async Task AddAsync(TranscodeJob job, CancellationToken ct) => await db.TranscodeJobs.AddAsync(job, ct);

    public Task<bool> HasActiveJobsAsync(Guid assetId, CancellationToken ct)
        => db.TranscodeJobs.AnyAsync(j => j.AssetId == assetId && (j.Status == TranscodeStatus.Pending || j.Status == TranscodeStatus.Running), ct);

    /// <summary>
    /// Added method to retrieve a TranscodeJob by its ID.
    /// This method uses the DbContext to find the entity by its primary key.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns>TranscodeJob object</returns>
    public async Task<TranscodeJob?> GetAsync(Guid id, CancellationToken ct)
    {
        return await db.TranscodeJobs.FindAsync([id], ct);
    }
}

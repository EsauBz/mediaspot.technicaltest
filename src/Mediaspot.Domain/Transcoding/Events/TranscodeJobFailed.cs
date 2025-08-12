using Mediaspot.Domain.Common;

namespace Mediaspot.Domain.Transcoding.Events
{
    /// <summary>
    /// Event that indicates a transcode job has failed.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TranscodeJobFailed"/> class.
    /// </remarks>
    /// <param name="jobId">The unique identifier of the transcode job that failed.</param
    public class TranscodeJobFailed(Guid jobId) : IDomainEvent
    {
        public Guid JobId { get; } = jobId;

        public DateTime OccurredOnUtc => DateTime.UtcNow;
    }
}
using Mediaspot.Domain.Common;

namespace Mediaspot.Domain.Transcoding.Events
{
    /// <summary>
    /// Event that indicates a transcode job has succeeded.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TranscodeJobSucceeded"/> class.
    /// </remarks>
    /// <param name="jobId">The unique identifier of the transcode job that succeeded.</param
    public class TranscodeJobSucceeded(Guid jobId) : IDomainEvent
    {
        public Guid JobId { get; } = jobId;

        public DateTime OccurredOnUtc => DateTime.UtcNow;
    }
}
using Mediaspot.Domain.Common;

namespace Mediaspot.Domain.Transcoding.Events
{
    /// <summary>
    /// Event that indicates a transcode job has started.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TranscodeJobStarted"/> class.
    /// </remarks>
    /// <param name="jobId">The unique identifier of the transcode job that started.</param
    public class TranscodeJobStarted(Guid jobId) : IDomainEvent
    {
        public Guid JobId { get; } = jobId;

        public DateTime OccurredOnUtc => DateTime.UtcNow;
    }
}
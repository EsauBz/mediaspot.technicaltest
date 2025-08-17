namespace Mediaspot.Application.Common.Queues;

/// <summary>
/// Interface for a queue that handles transcoding job requests.
/// </summary>
public interface ITranscodeQueue
{
    ValueTask EnqueueAsync(TranscodeJobRequest request);
    ValueTask<TranscodeJobRequest> DequeueAsync(CancellationToken ct);
}
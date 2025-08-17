using System.Threading.Channels;

namespace Mediaspot.Application.Common.Queues;

/// <summary>
/// In-memory implementation of the transcode queue.
/// </summary>
public class InMemoryTranscodeQueue : ITranscodeQueue
{
    private readonly Channel<TranscodeJobRequest> _channel = Channel.CreateUnbounded<TranscodeJobRequest>();

    public ValueTask EnqueueAsync(TranscodeJobRequest request) => _channel.Writer.WriteAsync(request);

    public ValueTask<TranscodeJobRequest> DequeueAsync(CancellationToken ct) => _channel.Reader.ReadAsync(ct);
}
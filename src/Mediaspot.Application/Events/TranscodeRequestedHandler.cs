using Mediaspot.Application.Common;
using Mediaspot.Application.Common.Queues;
using Mediaspot.Domain.Assets.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mediaspot.Application.Events;

/// <summary>
/// Handles the TranscodeRequested event
/// </summary>
/// <param name="queue"></param>
public sealed class TranscodeRequestedHandler(ITranscodeQueue queue, ILogger<TranscodeRequestedHandler> logger)
    : INotificationHandler<TranscodeRequested>
{
    public async Task Handle(TranscodeRequested @event, CancellationToken ct)
    {
        logger.LogInformation(
            "¡EVENT RECEIVED! TranscodeRequestedHandler was activated by Asset id: {AssetId}",
            @event.AssetId
        );

        var request = new TranscodeJobRequest(@event.AssetId, @event.MediaFileId, @event.TargetPreset);
        await queue.EnqueueAsync(request);
    }
}

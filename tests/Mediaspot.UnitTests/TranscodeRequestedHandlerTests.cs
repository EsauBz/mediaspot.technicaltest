using Mediaspot.Application.Events;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets.Events;
using Moq;
using Mediaspot.Application.Common.Queues;
using Microsoft.Extensions.Logging;

namespace Mediaspot.UnitTests;

public class TranscodeRequestedHandlerTests
{
    [Fact]
    public async Task Handle_Should_Add_TranscodeJob_And_Save()
    {
        var queue = new Mock<ITranscodeQueue>();
        queue.Setup(r => r.EnqueueAsync(It.IsAny<TranscodeJobRequest>())).Returns(ValueTask.CompletedTask);
        var logger = new Mock<ILogger<TranscodeRequestedHandler>>();
        var handler = new TranscodeRequestedHandler(queue.Object, logger.Object);
        var evt = new TranscodeRequested(Guid.NewGuid(), Guid.NewGuid(), "preset");

        await handler.Handle(evt, CancellationToken.None);

        queue.Verify(r => r.EnqueueAsync(It.Is<TranscodeJobRequest>(j => j.AssetId == evt.AssetId && j.MediaFileId == evt.MediaFileId && j.Preset == evt.TargetPreset)), Times.Once);
    }
}

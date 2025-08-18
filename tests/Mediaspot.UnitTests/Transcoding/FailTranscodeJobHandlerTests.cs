
using Mediaspot.Application.Common;
using Mediaspot.Application.Transcoding.Commands.Fail;
using Mediaspot.Domain.Transcoding;
using Moq;
using Shouldly;

namespace Mediaspot.UnitTests.Transcoding;

public class FailTranscodeJobHandlerTests
{
    [Fact]
    public async Task Handle_Should_MarkJobAsSucceded_And_SaveChanges()
    {
        // Arrange
        var job = new TranscodeJob(Guid.NewGuid(), Guid.NewGuid(), "1080p");
        var mockRepo = new Mock<ITranscodeJobRepository>();
        var mockUow = new Mock<IUnitOfWork>();
        var handler = new FailTranscodeJobHandler(mockRepo.Object, mockUow.Object);
        var command = new FailTranscodeJobCommand(job.Id, "This media file doesnt exist");

        // "Cuando te pidan este job, devuélvelo"
        mockRepo.Setup(r => r.GetAsync(job.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(job);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        // 1. Verificamos que el estado del objeto cambió
        job.Status.ShouldBe(TranscodeStatus.Failed);

        // 2. Verificamos que se llamó a guardar los cambios
        mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
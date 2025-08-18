
using Mediaspot.Application.Common;
using Mediaspot.Application.Transcoding.Commands.Complete;
using Mediaspot.Domain.Transcoding;
using Moq;
using Shouldly;

namespace Mediaspot.UnitTests.Transcoding;

public class CompleteTranscodeJobHandlerTests
{
    [Fact]
    public async Task Handle_Should_MarkJobAsSucceded_And_SaveChanges()
    {
        // Arrange
        var job = new TranscodeJob(Guid.NewGuid(), Guid.NewGuid(), "1080p");
        var mockRepo = new Mock<ITranscodeJobRepository>();
        var mockUow = new Mock<IUnitOfWork>();
        var handler = new CompleteTranscodeJobHandler(mockRepo.Object, mockUow.Object);
        var command = new CompleteTranscodeJobCommand(job.Id);

        // "Cuando te pidan este job, devuélvelo"
        mockRepo.Setup(r => r.GetAsync(job.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(job);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        // 1. Verificamos que el estado del objeto cambió
        job.Status.ShouldBe(TranscodeStatus.Succeeded);

        // 2. Verificamos que se llamó a guardar los cambios
        mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_KeyNotFoundException_When_Job_Not_Found()
    {
        // Arrange
        var mockRepo = new Mock<ITranscodeJobRepository>();
        var mockUow = new Mock<IUnitOfWork>();
        var handler = new CompleteTranscodeJobHandler(mockRepo.Object, mockUow.Object);
        var command = new CompleteTranscodeJobCommand(Guid.NewGuid());

        // "Cuando te pidan este job, devuelve nulo"
        mockRepo.Setup(r => r.GetAsync(command.JobId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TranscodeJob?)null);

        // Act & Assert
        await Should.ThrowAsync<KeyNotFoundException>(() =>
            handler.Handle(command, CancellationToken.None)
        );
    }
}
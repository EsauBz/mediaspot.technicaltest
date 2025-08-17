using Mediaspot.Domain.Transcoding;
using Mediaspot.Domain.Transcoding.Events;
using Shouldly;

namespace Mediaspot.UnitTests.Domain.Tests.Transcoding;

public class TranscodeJobTests
{
    [Fact]
    public void Constructor_Should_Set_Initial_State_To_Pending()
    {
        // Act
        var job = new TranscodeJob(Guid.NewGuid(), Guid.NewGuid(), "1080p");

        // Assert
        job.Status.ShouldBe(TranscodeStatus.Pending);
        job.CreatedAt.ShouldBeInRange(DateTime.UtcNow.AddSeconds(-1), DateTime.UtcNow);
        job.UpdatedAt.ShouldBeNull();
        job.DomainEvents.ShouldBeEmpty();
    }

    [Fact]
    public void MarkRunning_Should_Update_Status_And_Raise_Event()
    {
        // Arrange
        var job = new TranscodeJob(Guid.NewGuid(), Guid.NewGuid(), "1080p");

        // Act
        job.MarkRunning();

        // Assert
        job.Status.ShouldBe(TranscodeStatus.Running);
        job.UpdatedAt.ShouldNotBeNull();
        job.DomainEvents.OfType<TranscodeJobStarted>().Any().ShouldBeTrue();
    }

    [Fact]
    public void MarkSucceeded_Should_Update_Status_And_Raise_Event()
    {
        // Arrange
        var job = new TranscodeJob(Guid.NewGuid(), Guid.NewGuid(), "1080p");
        job.MarkRunning(); // A job must be running before it can succeed

        // Act
        job.MarkSucceeded();

        // Assert
        job.Status.ShouldBe(TranscodeStatus.Succeeded);
        job.DomainEvents.OfType<TranscodeJobStarted>().Any().ShouldBeTrue();
        job.DomainEvents.OfType<TranscodeJobSucceeded>().Any().ShouldBeTrue();
    }

    [Fact]
    public void MarkFailed_Should_Update_Status_And_Raise_Event()
    {
        // Arrange
        var job = new TranscodeJob(Guid.NewGuid(), Guid.NewGuid(), "1080p");
        job.MarkRunning();

        // Act
        job.MarkFailed();

        // Assert
        job.Status.ShouldBe(TranscodeStatus.Failed);
        job.DomainEvents.OfType<TranscodeJobStarted>().Any().ShouldBeTrue();
        job.DomainEvents.OfType<TranscodeJobFailed>().Any().ShouldBeTrue();
    }
}
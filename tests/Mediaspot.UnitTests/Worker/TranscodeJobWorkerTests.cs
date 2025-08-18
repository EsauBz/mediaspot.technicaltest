using Moq;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Mediaspot.Application.Common;
using Mediaspot.Application.Common.Queues;
using Mediaspot.Domain.Transcoding;
using Mediaspot.Worker;
using Mediaspot.Application.Transcoding.Commands.Start;
using Mediaspot.Application.Transcoding.Commands.Complete;

namespace Mediaspot.UnitTests.Worker;

public class TranscodeJobWorkerTests
{
    [Fact]
    public async Task ExecuteAsync_Should_Process_Job_From_Queue_Successfully()
    {
        // Arrange
        var mockQueue = new Mock<ITranscodeQueue>();
        var mockLogger = new Mock<ILogger<TranscodeJobWorker>>();
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockServiceScope = new Mock<IServiceScope>();
        var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
        var mockTaskDelayer = new Mock<ITaskDelayer>();

        var mockUow = new Mock<IUnitOfWork>();
        var mockJobRepo = new Mock<ITranscodeJobRepository>();
        var mockSender = new Mock<ISender>();

        // 1. We prepared the queue to return a job request that we will request
        var jobRequest = new TranscodeJobRequest(Guid.NewGuid(), Guid.NewGuid(), "1080p");
        mockQueue.SetupSequence(q => q.DequeueAsync(It.IsAny<CancellationToken>()))
             .ReturnsAsync(jobRequest)
             .Returns(new ValueTask<TranscodeJobRequest>(new TaskCompletionSource<TranscodeJobRequest>().Task));

        // 2. We configurate de service provider to return the necessary services that we mocked
        mockTaskDelayer.Setup(d => d.Delay(It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
                   .Returns(Task.CompletedTask);
        mockServiceProvider.Setup(sp => sp.GetService(typeof(IServiceScopeFactory))).Returns(mockServiceScopeFactory.Object);
        mockServiceScopeFactory.Setup(sf => sf.CreateScope()).Returns(mockServiceScope.Object);
        mockServiceScope.Setup(s => s.ServiceProvider.GetService(typeof(IUnitOfWork))).Returns(mockUow.Object);
        mockServiceScope.Setup(s => s.ServiceProvider.GetService(typeof(ITranscodeJobRepository))).Returns(mockJobRepo.Object);
        mockServiceScope.Setup(s => s.ServiceProvider.GetService(typeof(ISender))).Returns(mockSender.Object);

        // 3. We create the worker instance with the mocked dependencies
        var worker = new TranscodeJobWorker(mockServiceProvider.Object, mockQueue.Object, mockLogger.Object, mockTaskDelayer.Object);

        // We create a token that will cancel after some time so the test doesnt stuck in an infinite loop
        var cancellationTokenSource = new CancellationTokenSource();

        // Act
        // We execute the worker asynchronously
        _ = worker.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(100);
        cancellationTokenSource.Cancel();

        // Assert
        // We verify thqt the worker interacted with the mocked services as expected
        // 1. The job was created in DB
        mockJobRepo.Verify(r => r.AddAsync(It.IsAny<TranscodeJob>(), It.IsAny<CancellationToken>()), Times.Once);
        // 2. It sent the command to start the job
        mockSender.Verify(s => s.Send(It.IsAny<StartTranscodeJobCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        // 3. It sent the command to complete the job
        mockSender.Verify(s => s.Send(It.IsAny<CompleteTranscodeJobCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
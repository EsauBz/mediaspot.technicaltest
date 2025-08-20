using Mediaspot.Application.Assets.Commands.Create;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.ValueObjects;
using Moq;
using Shouldly;

namespace Mediaspot.UnitTests;

public class CreateAssetHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Asset_When_ExternalId_Is_Unique()
    {
        var repo = new Mock<IAssetRepository>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.GetByExternalIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((Asset?)null);
        repo.Setup(r => r.AddAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        var handler = new CreateAssetHandler(repo.Object, uow.Object);
        var cmd = new CreateAssetCommand("ext-unique", "title", "desc", "en", AssetType.Audio,
            DurationSeconds: 120, Bitrate: 320, SampleRate: 44100, Channels: 2, Resolution: null, FrameRate: null, Codec: null);

        var id = await handler.Handle(cmd, CancellationToken.None);

        id.ShouldNotBe(Guid.Empty);
        repo.Verify(r => r.AddAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>()), Times.Once);
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_ExternalId_Exists()
    {
        var repo = new Mock<IAssetRepository>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.GetByExternalIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(
            new VideoAsset("ext-unique", new Metadata("t", null, null), Duration.FromSeconds(120), "1920x1080", 30.0f, "H.264")
        );

        var handler = new CreateAssetHandler(repo.Object, uow.Object);
        var cmd = new CreateAssetCommand("ext-unique", "title", "desc", "en", AssetType.Audio,
                    DurationSeconds: 120, Bitrate: 320, SampleRate: 44100, Channels: 2, Resolution: null, FrameRate: null, Codec: null);

        await Should.ThrowAsync<InvalidOperationException>(() => handler.Handle(cmd, CancellationToken.None));
    }
}

using Mediaspot.Application.Assets.Commands.UpdateMetadata;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.ValueObjects;
using Moq;
using Shouldly;

namespace Mediaspot.UnitTests;

public class UpdateMetadataHandlerTests
{
    [Fact]
    public async Task Handle_Should_Update_MetadataForVideo_And_Save()
    {
        var asset = new VideoAsset("ext", new Metadata("t", null, null), Duration.FromSeconds(100), "1920x1080", 30.0f, "H.264");
        var repo = new Mock<IAssetRepository>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(asset);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        var handler = new UpdateMetadataHandler(repo.Object, uow.Object);
        var cmd = new UpdateMetadataCommand(asset.Id, "new", "desc", "fr");

        await handler.Handle(cmd, CancellationToken.None);

        asset.Metadata.Title.ShouldBe("new");
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Update_MetadataForAudio_And_Save()
    {
        var asset = new AudioAsset("ext", new Metadata("t", null, null), Duration.FromSeconds(1300), 180, 60, 3);
        var repo = new Mock<IAssetRepository>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(asset);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        var handler = new UpdateMetadataHandler(repo.Object, uow.Object);
        var cmd = new UpdateMetadataCommand(asset.Id, "new", "desc", "fr");

        await handler.Handle(cmd, CancellationToken.None);

        asset.Metadata.Title.ShouldBe("new");
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_If_Asset_Not_Found()
    {
        var repo = new Mock<IAssetRepository>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Asset?)null);
        var handler = new UpdateMetadataHandler(repo.Object, uow.Object);
        var cmd = new UpdateMetadataCommand(Guid.NewGuid(), "new", "desc", "fr");

        await Should.ThrowAsync<KeyNotFoundException>(() => handler.Handle(cmd, CancellationToken.None));
    }
}

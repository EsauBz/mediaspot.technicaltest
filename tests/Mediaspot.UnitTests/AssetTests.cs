using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.ValueObjects;
using Mediaspot.Domain.Assets.Events;
using Shouldly;

namespace Mediaspot.UnitTests;

public class AssetTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_And_Raise_AssetCreated()
    {
        var metadata = new Metadata("title", "desc", "en");
        var duration = Duration.FromSeconds(120);
        var frameRate = 30.0f;
        var codec = "H.264";
        var asset = new VideoAsset("ext-1", metadata, duration, "1920x1080", frameRate, codec);

        asset.ExternalId.ShouldBe("ext-1");
        asset.Metadata.ShouldBe(metadata);
        asset.Duration.ShouldBe(duration);
        asset.FrameRate.ShouldBe(frameRate);
        asset.Codec.ShouldBe(codec);
        asset.DomainEvents.OfType<AssetCreated>().Any(ac => ac.AssetId == asset.Id).ShouldBeTrue();
    }

    [Fact]
    public void RegisterMediaFile_Should_Add_File_And_Raise_Event()
    {
        var duration = Duration.FromSeconds(10);
        var bitrate = 320;
        var sampleRate = 44100;
        var channels = 2;
        var path = new FilePath("/file.mp3");
        var asset = new AudioAsset("ext-2", new Metadata("t", null, null), duration, bitrate, sampleRate, channels);

        var mf = asset.RegisterMediaFile(path, duration);

        asset.MediaFiles.ShouldContain(mf);
        asset.Bitrate.ShouldBe(bitrate);
        asset.SampleRate.ShouldBe(sampleRate);
        asset.Channels.ShouldBe(channels);
        asset.DomainEvents.OfType<MediaFileRegistered>().Any(reg => reg.AssetId == asset.Id && reg.MediaFileId == mf.Id.Value).ShouldBeTrue();
    }

    [Fact]
    public void UpdateMetadata_Should_Set_Metadata_And_Raise_Event()
    {
        var metadata = new Metadata("title", "desc", "en");
        var duration = Duration.FromSeconds(120);
        var frameRate = 30.0f;
        var codec = "H.264";
        var asset = new VideoAsset("ext-1", metadata, duration, "1920x1080", frameRate, codec);
        var newMeta = new Metadata("new", "d", "fr");

        asset.UpdateMetadata(newMeta);

        asset.Metadata.ShouldBe(newMeta);
        asset.DomainEvents.OfType<MetadataUpdated>().Any(mu => mu.AssetId == asset.Id).ShouldBeTrue();
    }

    [Fact]
    public void UpdateMetadata_Should_Throw_If_Title_Empty()
    {
        var metadata = new Metadata("title", "desc", "en");
        var duration = Duration.FromSeconds(120);
        var frameRate = 30.0f;
        var codec = "H.264";
        var asset = new VideoAsset("ext-1", metadata, duration, "1920x1080", frameRate, codec);
        var invalid = new Metadata("", null, null);

        Should.Throw<ArgumentException>(() => asset.UpdateMetadata(invalid));
    }

    [Fact]
    public void Archive_Should_Set_Archived_And_Raise_Event()
    {
        var metadata = new Metadata("title", "desc", "en");
        var duration = Duration.FromSeconds(120);
        var frameRate = 30.0f;
        var codec = "H.264";
        var asset = new VideoAsset("ext-1", metadata, duration, "1920x1080", frameRate, codec);
        asset.Archive(_ => false);

        asset.Archived.ShouldBeTrue();
        asset.DomainEvents.OfType<AssetArchived>().Any(aa => aa.AssetId == asset.Id).ShouldBeTrue();
    }

    [Fact]
    public void Archive_Should_Throw_If_ActiveJobs()
    {
        var metadata = new Metadata("title", "desc", "en");
        var duration = Duration.FromSeconds(120);
        var frameRate = 30.0f;
        var codec = "H.264";
        var asset = new VideoAsset("ext-1", metadata, duration, "1920x1080", frameRate, codec);
        Should.Throw<InvalidOperationException>(() => asset.Archive(_ => true));
    }

    [Fact]
    public void Archive_Should_Be_Idempotent()
    {
        var metadata = new Metadata("title", "desc", "en");
        var duration = Duration.FromSeconds(120);
        var frameRate = 30.0f;
        var codec = "H.264";
        var asset = new VideoAsset("ext-1", metadata, duration, "1920x1080", frameRate, codec);
        asset.Archive(_ => false);
        asset.Archive(_ => false);
        asset.Archived.ShouldBeTrue();
        asset.DomainEvents.OfType<AssetArchived>().Count().ShouldBe(1);
    }
}

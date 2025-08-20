using Mediaspot.Domain.Assets.ValueObjects;

namespace Mediaspot.Domain.Assets;

/// <summary>
/// Represents a video asset in the system that is an extension of the Asset class.
/// </summary>
public class VideoAsset : Asset
{
    public Duration Duration { get; private set; } = Duration.FromSeconds(0);
    public string Resolution { get; private set; } = string.Empty;
    public float FrameRate { get; private set; }
    public string Codec { get; private set; } = string.Empty;

    public VideoAsset(string externalId, Metadata metadata, Duration duration, string resolution, float frameRate, string codec)
        : base(externalId, metadata)
    {
        Duration = duration;
        Resolution = resolution;
        FrameRate = frameRate;
        Codec = codec;
    }

    private VideoAsset() { }
}
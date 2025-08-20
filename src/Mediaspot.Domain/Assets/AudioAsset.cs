using Mediaspot.Domain.Assets.ValueObjects;

namespace Mediaspot.Domain.Assets;

/// <summary>
/// Represents an audio asset in the system that is an extension of the Asset class.
/// </summary>
public class AudioAsset : Asset
{
    public Duration Duration { get; private set; } = Duration.FromSeconds(0);
    public int Bitrate { get; private set; } = 0;
    public int SampleRate { get; private set; } = 0;
    public int Channels { get; private set; } = 0;

    public AudioAsset(string externalId, Metadata metadata, Duration duration, int bitrate, int sampleRate, int channels)
        : base(externalId, metadata)
    {
        Duration = duration;
        Bitrate = bitrate;
        SampleRate = sampleRate;
        Channels = channels;
    }

    private AudioAsset() { }
}
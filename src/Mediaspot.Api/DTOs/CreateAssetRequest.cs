
using Mediaspot.Application.Common;

namespace Mediaspot.Api.DTOs;


public class CreateAssetRequest
{
    public required string ExternalId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Language { get; set; }
    public AssetType Type { get; set; }
    public double? DurationSeconds { get; set; }

    // Video-specific properties (nullable)
    public string? Resolution { get; set; }
    public float? FrameRate { get; set; }
    public string? Codec { get; set; }

    // Audio-specific properties (nullable)
    public int? Bitrate { get; set; }
    public int? SampleRate { get; set; }
    public int? Channels { get; set; }
}
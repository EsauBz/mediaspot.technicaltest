namespace Mediaspot.Application.Common;

public record TranscodeJobRequest(Guid AssetId, Guid MediaFileId, string Preset);

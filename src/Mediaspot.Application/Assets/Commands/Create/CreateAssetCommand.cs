using Mediaspot.Application.Common;
using MediatR;

namespace Mediaspot.Application.Assets.Commands.Create;

public sealed record CreateAssetCommand(
    string ExternalId,
    string Title,
    string? Description,
    string? Language,
    AssetType Type,
    double? DurationSeconds,
    string? Resolution,
    float? FrameRate,
    string? Codec,
    int? Bitrate,
    int? SampleRate,
    int? Channels
) : IRequest<Guid>;
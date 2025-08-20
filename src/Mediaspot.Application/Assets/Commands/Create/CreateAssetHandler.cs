using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Assets.ValueObjects;
using MediatR;

namespace Mediaspot.Application.Assets.Commands.Create;

public sealed class CreateAssetHandler(IAssetRepository repo, IUnitOfWork uow)
    : IRequestHandler<CreateAssetCommand, Guid>
{
    public async Task<Guid> Handle(CreateAssetCommand request, CancellationToken ct)
    {
        // Enforce uniqueness of ExternalId
        var existing = await repo.GetByExternalIdAsync(request.ExternalId, ct);
        if (existing is not null)
            throw new InvalidOperationException($"Asset with ExternalId '{request.ExternalId}' already exists.");

        var metadata = new Metadata(request.Title, request.Description, request.Language);
        Asset newAsset;
        var duration = Duration.FromSeconds(request.DurationSeconds ?? 0);

        switch (request.Type)
        {
            case AssetType.Video:
                if (string.IsNullOrEmpty(request.Resolution) || !request.FrameRate.HasValue || string.IsNullOrEmpty(request.Codec))
                {
                    throw new ArgumentException("Resolution, FrameRate, and Codec are required for Video assets.");
                }
                newAsset = new VideoAsset(
                    request.ExternalId,
                    metadata,
                    duration,
                    request.Resolution,
                    request.FrameRate.Value,
                    request.Codec
                );
                break;

            case AssetType.Audio:
                // Validate that audio-specific fields are present
                if (!request.Bitrate.HasValue || !request.SampleRate.HasValue || !request.Channels.HasValue)
                {
                    throw new ArgumentException("Bitrate, SampleRate, and Channels are required for Audio assets.");
                }
                newAsset = new AudioAsset(
                    request.ExternalId,
                    metadata,
                    duration,
                    request.Bitrate.Value,
                    request.SampleRate.Value,
                    request.Channels.Value
                );
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(request.Type), "Unsupported asset type.");
        }

        await repo.AddAsync(newAsset, ct);
        await uow.SaveChangesAsync(ct);
        return newAsset.Id;
    }
}

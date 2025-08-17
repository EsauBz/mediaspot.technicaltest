using Mediaspot.Domain.Common;

namespace Mediaspot.Domain.Assets.Events;

public sealed record MediaFileRegistered(Guid AssetId, Guid MediaFileId) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

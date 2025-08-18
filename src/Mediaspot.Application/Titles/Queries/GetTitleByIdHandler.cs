using Mediaspot.Application.Common;
using Mediaspot.Domain.Titles;
using MediatR;

namespace Mediaspot.Application.Titles.Queries;

/// <summary>
/// The Query: The data needed to get a title (its ID).
/// </summary>
/// <param name="titleRepository"></param>
public sealed class GetTitleByIdQueryHandler(ITitleRepository titleRepository)
    : IRequestHandler<GetTitleByIdQuery, Title?>
{
    public async Task<Title?> Handle(GetTitleByIdQuery request, CancellationToken ct)
    {
        return await titleRepository.GetByIdAsync(request.Id, ct);
    }
}
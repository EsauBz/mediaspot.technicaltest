using MediatR;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Titles;

namespace Mediaspot.Application.Titles.Queries;

/// <summary>
/// The Query: The data needed to get all titles.
/// It returns a collection of Title objects.
/// </summary>
/// <param name="titleRepository"></param>
public sealed class GetAllTitlesQueryHandler(ITitleRepository titleRepository)
    : IRequestHandler<GetAllTitlesQuery, IEnumerable<Title>>
{
    public async Task<IEnumerable<Title>> Handle(GetAllTitlesQuery request, CancellationToken ct)
    {
        return await titleRepository.GetAllAsync(ct);
    }
}
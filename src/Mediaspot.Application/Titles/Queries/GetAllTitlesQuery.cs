using Mediaspot.Domain.Titles;
using MediatR;

namespace Mediaspot.Application.Titles.Queries;
/// <summary>
/// The Query: No parameters needed, it just requests all titles.
/// </summary>
public sealed record GetAllTitlesQuery() : IRequest<IEnumerable<Title>>;

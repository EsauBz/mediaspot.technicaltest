using Mediaspot.Domain.Titles;
using MediatR;

namespace Mediaspot.Application.Titles.Commands.Create;

/// <summary>
/// Entity that represents the create title command.
/// </summary>
/// <param name="Name"></param>
/// <param name="Description"></param>
/// <param name="ReleaseDate"></param>
/// <param name="Type"></param>
public sealed record CreateTitleCommand(
    string Name,
    string Description,
    DateTime ReleaseDate,
    TitleType Type
) : IRequest<Guid>;
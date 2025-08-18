using MediatR;
using Mediaspot.Application.Common;
using Mediaspot.Domain.Titles;

namespace Mediaspot.Application.Titles.Commands.Create;

/// <summary>
/// Command to create a new title.
/// </summary>
/// <param name="Name">The name of the title.</param>
/// <param name="Description">A brief description of the title.</param>
/// <param name="ReleaseDate">The release date of the title.</param>
/// <param name="Type">The type of the title (e.g., Movie, Series).</param>
/// </summary>
/// <param name="titleRepository"></param>
/// <param name="uow"></param>
public sealed class CreateTitleCommandHandler(ITitleRepository titleRepository, IUnitOfWork uow)
    : IRequestHandler<CreateTitleCommand, Guid>
{
    public async Task<Guid> Handle(CreateTitleCommand request, CancellationToken ct)
    {
        var nameExists = await titleRepository.DoesNameExistAsync(request.Name, ct);
        if (nameExists)
        {
            throw new InvalidOperationException("A title with the same name already exists.");
        }
        if (!Enum.IsDefined(typeof(TitleType), request.Type))
        {
            throw new InvalidOperationException($"The value '{request.Type}' is not a valid title type.");
        }

        var newTitle = new Title(
            request.Name,
            request.Description,
            request.ReleaseDate,
            request.Type
        );

        await titleRepository.AddAsync(newTitle, ct);
        await uow.SaveChangesAsync(ct);

        return newTitle.Id;
    }
}
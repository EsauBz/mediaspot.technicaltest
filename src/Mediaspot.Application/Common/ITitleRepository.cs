using Mediaspot.Domain.Titles;

namespace Mediaspot.Application.Common;

public interface ITitleRepository
{
    /// <summary>
    /// Adds a new title to the repository.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task AddAsync(Title title, CancellationToken ct);
    /// <summary>
    /// Checks if a title with the given name already exists in the repository.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<bool> DoesNameExistAsync(string name, CancellationToken ct);
    /// <summary>
    /// Retrieves a title by its ID from the repository.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<Title?> GetByIdAsync(Guid id, CancellationToken ct);
    /// <summary>
    /// Retrieves all titles from the repository.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<IEnumerable<Title>> GetAllAsync(CancellationToken ct);
}
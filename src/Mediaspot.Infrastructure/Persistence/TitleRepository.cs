using Mediaspot.Application.Common;
using Mediaspot.Domain.Titles;
using Microsoft.EntityFrameworkCore;

namespace Mediaspot.Infrastructure.Persistence;

/// <summary>
/// TitleRepository is responsible for managing Title entities in the database.
/// It implements the ITitleRepository interface to provide methods for adding titles and checking for existing names.
/// This repository uses Entity Framework Core to interact with the MediaspotDbContext.
/// </summary>
/// <param name="context"></param>
public class TitleRepository(MediaspotDbContext context) : ITitleRepository
{
    /// inheritdoc />
    public async Task AddAsync(Title title, CancellationToken ct)
    {
        await context.Titles.AddAsync(title, ct);
    }

    /// inheritdoc />
    public async Task<bool> DoesNameExistAsync(string name, CancellationToken ct)
    {
        return await context.Titles.AnyAsync(t => t.Name == name, ct);
    }

    /// inheritdoc />
    public async Task<Title?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await context.Titles.FindAsync(new object[] { id }, ct);
    }

    /// inheritdoc />
    public async Task<IEnumerable<Title>> GetAllAsync(CancellationToken ct)
    {
        return await context.Titles.ToListAsync(ct);
    }
}
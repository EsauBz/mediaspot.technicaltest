using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mediaspot.Infrastructure.Persistence;

public class MediaspotDbContextFactory : IDesignTimeDbContextFactory<MediaspotDbContext>
{
    public MediaspotDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MediaspotDbContext>();
        optionsBuilder.UseInMemoryDatabase("Mediaspot.Backend.TechnicalTest");

        return new MediaspotDbContext(optionsBuilder.Options);
    }
}
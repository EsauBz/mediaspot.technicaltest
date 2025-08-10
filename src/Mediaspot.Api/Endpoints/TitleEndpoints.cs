using Mediaspot.Api.DTOs;
using Mediaspot.Domain.Titles;
using Mediaspot.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mediaspot.API.Endpoints
{
    public static class TitleEndpoints
    {
        public static IEndpointRouteBuilder MapTitleEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/titles").WithTags("Titles");

            // POST /api/titles
            group.MapPost("/", async ([FromBody] CreateTitleRequest request, MediaspotDbContext context) =>
            {
                var nameExists = await context.Titles.AnyAsync(t => t.Name == request.Name);
                if (nameExists)
                {
                    return Results.Conflict("A title with the same name already exists.");
                }

                if (!Enum.IsDefined(typeof(TitleType), request.Type))
                {
                    return Results.BadRequest($"The value '{request.Type}' is not a valid title type.");
                }

                var newTitle = new Title(request.Name, request.Description, request.ReleaseDate, request.Type);

                context.Titles.Add(newTitle);
                await context.SaveChangesAsync();

                return Results.CreatedAtRoute("GetTitleById", new { id = newTitle.Id }, newTitle);
            });

            // GET /api/titles/{id}
            group.MapGet("/{id:guid}", async (Guid id, MediaspotDbContext context) =>
            {
                var title = await context.Titles.FindAsync(id);
                return title is not null ? Results.Ok(title) : Results.NotFound();
            })
            .WithName("GetTitleById"); // Le damos un nombre para usarlo en CreatedAtRoute

            // GET /api/titles
            group.MapGet("/", async (MediaspotDbContext context) =>
            {
                var titles = await context.Titles.ToListAsync();
                return Results.Ok(titles);
            });

            return app;
        }
    }
}
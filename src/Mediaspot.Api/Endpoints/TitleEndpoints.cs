using Mediaspot.Api.DTOs;
using Mediaspot.Application.Titles.Commands.Create;
using Mediaspot.Application.Titles.Queries;
using MediatR;

namespace Mediaspot.API.Endpoints
{
    public static class TitleEndpoints
    {
        public static IEndpointRouteBuilder MapTitleEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/titles").WithTags("Titles");

            // POST /api/titles
            group.MapPost("/", async (CreateTitleRequest request, ISender sender) =>
            {
                // 1. Create the command from the incoming request data.
                var command = new CreateTitleCommand(
                    request.Name,
                    request.Description,
                    request.ReleaseDate,
                    request.Type
                );

                // 2. Send the command to MediatR. MediatR will find the correct handler and execute it.
                var titleId = await sender.Send(command);

                // 3. Return the 201 Created response.
                return Results.CreatedAtRoute("GetTitleById", new { id = titleId }, new { id = titleId });
            });

            // GET /api/titles/{id}
            group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var title = await sender.Send(new GetTitleByIdQuery(id));
                return title is not null ? Results.Ok(title) : Results.NotFound();
            })
            .WithName("GetTitleById");

            // GET /api/titles
            group.MapGet("/", async (ISender sender) =>
            {
                var titles = await sender.Send(new GetAllTitlesQuery());
                return Results.Ok(titles);
            });

            return app;
        }
    }
}
using Mediaspot.Api.DTOs;
using Mediaspot.Application.Assets.Commands.Archive;
using Mediaspot.Application.Assets.Commands.Create;
using Mediaspot.Application.Assets.Commands.RegisterMediaFile;
using Mediaspot.Application.Assets.Commands.UpdateMetadata;
using Mediaspot.Application.Assets.Queries.GetById;
using MediatR;

namespace Mediaspot.API.Endpoints
{
    public static class AssetEndpoints
    {
        // Este es un método de extensión para IEndpointRouteBuilder (el 'app')
        public static IEndpointRouteBuilder MapAssetEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/assets");

            app.MapGet("/assets/{id:guid}", async (Guid id, ISender sender) =>
            {
                var asset = await sender.Send(new GetAssetByIdQuery(id));
                return Results.Ok(asset);
            })
            .WithName("GetAssetById")
            .WithOpenApi();

            app.MapPost("/assets", async (CreateAssetCommand cmd, ISender sender) => Results.Created("/assets", new { id = await sender.Send(cmd) }))
                .WithName("PostCreateAsset")
                .WithOpenApi();

            app.MapPost("/assets/{id:guid}/files", async (Guid id, string path, double durationSeconds, ISender sender)
                    => Results.Ok(new { mediaFileId = await sender.Send(new RegisterMediaFileCommand(id, path, durationSeconds)) }))
                .WithName("PostRegisterMediaFile")
                .WithOpenApi();

            app.MapPut("/assets/{id:guid}/metadata", async (Guid id, UpdateMetadataDto dto, ISender sender) =>
                {
                    await sender.Send(new UpdateMetadataCommand(id, dto.Title, dto.Description, dto.Language));
                    return Results.NoContent();
                })
                .WithName("PutUpdateMetadata")
                .WithOpenApi();

            app.MapPost("/assets/{id:guid}/archive", async (Guid id, ISender sender) =>
                {
                    await sender.Send(new ArchiveAssetCommand(id));
                    return Results.NoContent();
                })
                .WithName("PostArchiveAsset")
                .WithOpenApi();

            return app;
        }
    }
}
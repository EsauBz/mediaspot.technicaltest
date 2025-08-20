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

            app.MapPost("/assets", async (CreateAssetCommand request, ISender sender, ILogger<Program> logger) =>
            {
                var command = new CreateAssetCommand(
                    request.ExternalId,
                    request.Title,
                    request.Description,
                    request.Language,
                    request.Type,
                    request.DurationSeconds,
                    request.Resolution,
                    request.FrameRate,
                    request.Codec,
                    request.Bitrate,
                    request.SampleRate,
                    request.Channels
                );
                logger.LogInformation("Creating asset with ExternalId: {ExternalId}", request.ExternalId);

                try
                {
                    var assetId = await sender.Send(command);
                    return Results.Created($"/assets/{assetId}", new { id = assetId });
                }
                catch (InvalidOperationException ex)
                {
                    logger.LogWarning("--> Conflict in creating asset: {ErrorMessage}", ex.Message);
                    return Results.Conflict(new { message = ex.Message });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "--> Inexpected error creating Asset.");
                    return Results.Problem("An unexpected error occurred.");
                }
            })
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
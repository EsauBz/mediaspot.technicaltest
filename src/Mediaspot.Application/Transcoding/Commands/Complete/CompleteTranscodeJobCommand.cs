using MediatR;

namespace Mediaspot.Application.Transcoding.Commands.Complete;

public sealed record CompleteTranscodeJobCommand(Guid JobId) : IRequest;

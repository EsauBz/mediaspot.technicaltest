using MediatR;

namespace Mediaspot.Application.Transcoding.Commands.Start
{
    public sealed record StartTranscodeJobCommand(Guid JobId) : IRequest;
}
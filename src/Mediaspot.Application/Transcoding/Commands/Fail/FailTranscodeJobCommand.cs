using MediatR;

namespace Mediaspot.Application.Transcoding.Commands.Fail
{
    public sealed record FailTranscodeJobCommand(Guid JobId, string ErrorMessage) : IRequest;
}

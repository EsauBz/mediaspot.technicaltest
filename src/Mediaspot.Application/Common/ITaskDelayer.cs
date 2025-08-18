namespace Mediaspot.Application.Common;

public interface ITaskDelayer
{
    Task Delay(TimeSpan duration, CancellationToken ct);
}

public class TaskDelayer : ITaskDelayer
{
    public Task Delay(TimeSpan duration, CancellationToken ct)
    {
        return Task.Delay(duration, ct);
    }
}
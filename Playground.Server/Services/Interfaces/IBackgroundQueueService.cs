namespace Playground.Server.Services.Interfaces;

public interface IBackgroundQueueService
{
    Task QueueAsync(Func<CancellationToken, Task> action);

    IAsyncEnumerable<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken = default);
}

using System.Threading.Channels;

namespace Budgeteer.Server.Services;

public class BackgroundQueueService : IBackgroundQueueService
{
    private readonly Channel<Func<CancellationToken, Task>> _channel;

    public BackgroundQueueService()
    {
        _channel = Channel.CreateUnbounded<Func<CancellationToken, Task>>(new()
        {
            SingleReader = true,
            SingleWriter = false
        });
    }

    public async Task QueueAsync(Func<CancellationToken, Task> action) =>
        await _channel.Writer.WriteAsync(action);

    public IAsyncEnumerable<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken = default) =>
        _channel.Reader.ReadAllAsync(cancellationToken);
}

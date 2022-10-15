using System.Threading.Channels;

namespace Playground.Server.Services;

public class BackgroundQueueService
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

    public async Task QueueAsync(
        Func<CancellationToken, Task> action)
    {
        await _channel.Writer.WriteAsync(action);
    }

    public IAsyncEnumerable<Func<CancellationToken, Task>> DequeueAsync(
        CancellationToken cancellationToken = default)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}

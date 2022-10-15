namespace Playground.Server.Services;

public class BackgroundWorkerService : BackgroundService
{
    private readonly BackgroundQueueService _queueService;

    public BackgroundWorkerService(BackgroundQueueService queueService)
    {
        _queueService = queueService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var action in _queueService.DequeueAsync(stoppingToken))
        {
            await action(stoppingToken);
        }
    }
}

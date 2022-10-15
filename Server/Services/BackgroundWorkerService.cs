namespace Playground.Server.Services;

public class BackgroundWorkerService : BackgroundService
{
    private readonly BackgroundQueueService _backgroundQueueService;

    public BackgroundWorkerService(
        BackgroundQueueService backgroundQueueService)
    {
        _backgroundQueueService = backgroundQueueService;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        await foreach (var action in _backgroundQueueService.DequeueAsync(stoppingToken))
        {
            await action(stoppingToken);
        }
    }
}

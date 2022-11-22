namespace Budgeteer.Server.Services;

public class BackgroundWorkerService : BackgroundService
{
    private readonly IBackgroundQueueService _backgroundQueueService;

    public BackgroundWorkerService(IBackgroundQueueService backgroundQueueService)
    {
        _backgroundQueueService = backgroundQueueService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var action in _backgroundQueueService.DequeueAsync(stoppingToken))
            await action(stoppingToken);
    }
}

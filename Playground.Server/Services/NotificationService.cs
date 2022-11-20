using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Playground.Server.Services;

public class NotificationService : INotificationService
{
    private readonly PlaygroundContext _context;
    private readonly IBackgroundQueueService _backgroundQueueService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;

    public NotificationService(
        IDbContextFactory<PlaygroundContext> contextFactory,
        IBackgroundQueueService backgroundQueueService,
        IServiceScopeFactory serviceScopeFactory,
        IMapper mapper)
    {
        _context = contextFactory.CreateDbContext();
        _backgroundQueueService = backgroundQueueService;
        _serviceScopeFactory = serviceScopeFactory;
        _mapper = mapper;
    }

    public async Task SendIndividualAsync(
        int userId, string message, CancellationToken cancellationToken = default)
    {
        await _backgroundQueueService.QueueAsync(async (cancellationToken) =>
        {
            using var scope = _serviceScopeFactory.CreateScope();

            using (var context = await scope.ServiceProvider
                .GetRequiredService<IDbContextFactory<PlaygroundContext>>()
                .CreateDbContextAsync(cancellationToken))
            {
                context.Notifications.Create(new()
                {
                    Message = message,
                    UserId = userId
                }, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);
            }

            var notificationHub = scope.ServiceProvider
                .GetRequiredService<IHubContext<NotificationHub>>();

            await notificationHub.Clients
                .User(userId.ToString())
                .SendAsync("Notification", cancellationToken);
        });
    }

    public async Task<ICollection<NotificationResponse>> GetNotificationsAsync(
        int userId, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .GetAllAsync<Notification, NotificationResponse>(_mapper.ConfigurationProvider, cancellationToken: cancellationToken);
    }

    public async Task ReadNotificationAsync(
        int id, CancellationToken cancellationToken = default)
    {
        var notification = await _context.Notifications.GetByIdAsync(id, cancellationToken: cancellationToken);

        if (notification != null)
        {
            notification.ReadDateTime = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

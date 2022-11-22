using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace Budgeteer.Server.Services;

public class NotificationService : INotificationService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IBackgroundQueueService _backgroundQueueService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;

    public NotificationService(
        UnitOfWork unitOfWork,
        IBackgroundQueueService backgroundQueueService,
        IServiceScopeFactory serviceScopeFactory,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _backgroundQueueService = backgroundQueueService;
        _serviceScopeFactory = serviceScopeFactory;
        _mapper = mapper;
    }

    public async Task SendIndividualAsync(
        int userId, string message, CancellationToken cancellationToken = default)
    {
        await _backgroundQueueService.QueueAsync(async cancellationToken =>
        {
            using var scope = _serviceScopeFactory.CreateScope();

            await using (var unitOfWork = scope.ServiceProvider
                .GetRequiredService<UnitOfWork>())
            {
                unitOfWork.Notifications.Create(new()
                {
                    Message = message,
                    UserId = userId
                });

                await unitOfWork.SaveAsync(cancellationToken);
            }

            var notificationHub = scope.ServiceProvider
                .GetRequiredService<IHubContext<NotificationHub>>();

            await notificationHub.Clients
                .User(userId.ToString())
                .SendAsync("Notification", cancellationToken);
        });
    }

    public async Task<ICollection<NotificationResponse>> GetNotificationsAsync(int userId, CancellationToken cancellationToken = default) =>
        await _unitOfWork.Notifications.GetAllAsync<NotificationResponse>(_mapper.ConfigurationProvider, cancellationToken: cancellationToken);

    public async Task ReadNotificationAsync(int id, CancellationToken cancellationToken = default)
    {
        var notification = await _unitOfWork.Notifications
            .GetAsync(id, _mapper.ConfigurationProvider, cancellationToken: cancellationToken);

        if (notification != null)
        {
            notification.ReadDateTime = DateTime.UtcNow;
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}

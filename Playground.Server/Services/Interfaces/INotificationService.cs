namespace Playground.Server.Services.Interfaces;

public interface INotificationService
{
    Task SendIndividualAsync(int userId, string message, CancellationToken cancellationToken = default);

    Task<ICollection<NotificationResponse>> GetNotificationsAsync(int userId, CancellationToken cancellationToken = default);

    Task ReadNotificationAsync(int id, CancellationToken cancellationToken = default);
}

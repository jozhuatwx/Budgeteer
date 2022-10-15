namespace Playground.Infrastructure.DTOs;

public record NotificationResponse(DateTime CreatedDateTime, string Message, bool IsRead);

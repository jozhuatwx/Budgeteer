namespace Playground.Shared.DTOs;

public record NotificationResponse(DateTime CreatedDateTime, string Message, bool IsRead);

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Playground.Server.Controllers;

[ApiController, Route("[controller]"), Authorize]
public class NotificationsController : Controller
{
    private readonly NotificationService _notificationService;

    public NotificationsController(
        NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotificationsAsync(
        CancellationToken cancellationToken)
    {
        if (HttpContext.User.TryGetId(out var userId))
        {
            return Ok(await _notificationService.GetNotificationsAsync(userId, cancellationToken));
        }
        return BadRequest();
    }
}

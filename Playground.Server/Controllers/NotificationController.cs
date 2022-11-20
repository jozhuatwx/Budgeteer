using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Playground.Server.Controllers;

[ApiController, Route("[controller]"), Authorize]
[Produces(MediaTypeNames.Application.Json), ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class NotificationController : Controller
{
    private readonly NotificationService _notificationService;

    public NotificationController(
        NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("/All")]
    [ProducesResponseType(typeof(ICollection<NotificationResponse>), StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetNotificationsAsync(
        CancellationToken cancellationToken)
    {
        if (HttpContext.User.TryGetId(out var userId))
        {
            return Ok(await _notificationService.GetNotificationsAsync(userId, cancellationToken));
        }
        return BadRequest();
    }

    [HttpPut("/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateNotificationAsync(
        int id, CancellationToken cancellationToken)
    {
        await _notificationService.ReadNotificationAsync(id, cancellationToken);
        return Ok();
    }
}

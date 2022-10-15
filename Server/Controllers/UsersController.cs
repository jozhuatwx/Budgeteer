using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Playground.Server.Controllers;

[ApiController, Route("[controller]"), Authorize]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly SessionService _sessionService;

    public UsersController(
        UserService userService,
        SessionService sessionService)
    {
        _userService = userService;
        _sessionService = sessionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsersAsync(
        CancellationToken cancellationToken)
    {
        return Ok(await _userService.GetUsersAsync(cancellationToken));
    }

    [HttpPost("User"), AllowAnonymous]
    public async Task<IActionResult> CreateUserAsync(
        CreateUserRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _userService.CreateUserAsync(request, cancellationToken));
    }

    [HttpGet("User")]
    public async Task<IActionResult> GetUserAsync(
        CancellationToken cancellationToken)
    {
        return Ok(await _userService.GetUserAsync(HttpContext.User, cancellationToken));
    }

    [HttpPut("User")]
    public async Task<IActionResult> UpdateUserAsync(
        UpdateUserRequest request, CancellationToken cancellationToken)
    {
        if (HttpContext.User.TryGetId(out var id))
        {
            return Ok(await _userService.UpdateUserAsync(id, request, cancellationToken));
        }
        return BadRequest();
    }

    [HttpDelete("User")]
    public async Task<IActionResult> DeleteUserAsync(
        CancellationToken cancellationToken)
    {
        if (HttpContext.User.TryGetId(out var id))
        {
            return Ok(await _userService.DeleteUserAsync(id, cancellationToken));
        }
        return BadRequest();
    }

    [HttpPost("User/Login"), AllowAnonymous]
    public async Task<IActionResult> LoginUserAsync(
        LoginUserRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _sessionService.LoginUserAsync(request, cancellationToken));
    }

    [HttpPost("User/RefreshSession")]
    public async Task<IActionResult> RefreshUserSessionAsync(
        RefreshUserSessionRequest request, CancellationToken cancellationToken)
    {
        if (HttpContext.User.TryGetId(out var id))
        {
            return Ok(await _sessionService.RefreshUserSessionAsync(id, request, cancellationToken));
        }
        return BadRequest();
    }
}

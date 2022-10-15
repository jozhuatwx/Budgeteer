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
    public async Task<IActionResult> GetUsersAsync()
    {
        return Ok(await _userService.GetUsersAsync());
    }

    [HttpPost("User"), AllowAnonymous]
    public async Task<IActionResult> CreateUserAsync(
        CreateUserRequest request)
    {
        return Ok(await _userService.CreateUserAsync(request));
    }

    [HttpGet("User")]
    public async Task<IActionResult> GetUserAsync()
    {
        return Ok(await _userService.GetUserAsync(HttpContext.User));
    }

    [HttpPut("User")]
    public async Task<IActionResult> UpdateUserAsync(
        UpdateUserRequest request)
    {
        if (HttpContext.User.TryGetUserId(out var id))
        {
            return Ok(await _userService.UpdateUserAsync(id, request));
        }
        return BadRequest();
    }

    [HttpDelete("User")]
    public async Task<IActionResult> DeleteUserAsync()
    {
        if (HttpContext.User.TryGetUserId(out var id))
        {
            return Ok(await _userService.DeleteUserAsync(id));
        }
        return BadRequest();
    }

    [HttpPost("User/Login"), AllowAnonymous]
    public async Task<IActionResult> LoginUserAsync(
        LoginUserRequest request)
    {
        return Ok(await _sessionService.LoginUserAsync(request));
    }

    [HttpPost("User/RefreshSession")]
    public async Task<IActionResult> RefreshUserSessionAsync(
        RefreshUserSessionRequest request)
    {
        if (HttpContext.User.TryGetUserId(out var id))
        {
            return Ok(await _sessionService.RefreshUserSessionAsync(id, request));
        }
        return BadRequest();
    }
}

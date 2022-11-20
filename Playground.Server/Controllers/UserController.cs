using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Playground.Server.Controllers;

[ApiController, Route("[controller]"), Authorize]
[Produces(MediaTypeNames.Application.Json), ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly SessionService _sessionService;

    public UserController(
        UserService userService,
        SessionService sessionService)
    {
        _userService = userService;
        _sessionService = sessionService;
    }

    [HttpGet("All")]
    [ProducesResponseType(typeof(ICollection<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsersAsync(
        CancellationToken cancellationToken)
    {
        return Ok(await _userService.GetUsersAsync(cancellationToken));
    }

    [HttpPost, AllowAnonymous]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateUserAsync(
        CreateUserRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _userService.CreateUserAsync(request, cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserAsync(
        CancellationToken cancellationToken)
    {
        return Ok(await _userService.GetUserAsync(HttpContext.User, cancellationToken));
    }

    [HttpPut]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserAsync(
        UpdateUserRequest request, CancellationToken cancellationToken)
    {
        if (HttpContext.User.TryGetId(out var id))
        {
            return Ok(await _userService.UpdateUserAsync(id, request, cancellationToken));
        }
        return BadRequest();
    }

    [HttpDelete]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUserAsync(
        CancellationToken cancellationToken)
    {
        if (HttpContext.User.TryGetId(out var id))
        {
            return Ok(await _userService.DeleteUserAsync(id, cancellationToken));
        }
        return BadRequest();
    }

    [HttpPost("Login"), AllowAnonymous]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginUserAsync(
        LoginUserRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _sessionService.LoginUserAsync(request, cancellationToken));
    }

    [HttpPost("RefreshSession")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
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

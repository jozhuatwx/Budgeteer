using System.ComponentModel.DataAnnotations;

namespace Playground.Shared.DTOs;

public record CreateUserRequest([Required] string Name, [Required, EmailAddress] string Email, [Required, StringLength(50, MinimumLength = 8)] string Password);
public record UpdateUserRequest([Required] string Name, [Required, EmailAddress] string Email);
public record LoginUserRequest([Required] string Email, [Required] string Password);
public record RefreshUserSessionRequest([Required] string RefreshToken);

public record UserResponse(int Id, string Name, string Email);
public record UserSessionResponse(string Token, string RefreshToken);

namespace Playground.Infrastructure.DTOs;

public record CreateUserRequest(string Name, string Email, string Password);
public record UpdateUserRequest(string Name, string Email);
public record LoginUserRequest(string Email, string Password);
public record RefreshUserSessionRequest(string RefreshToken);

public record UserResponse(int Id, string Name, string Email);
public record UserSessionResponse(string Token, string RefreshToken);

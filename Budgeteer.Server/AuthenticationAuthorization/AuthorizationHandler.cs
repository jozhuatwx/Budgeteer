using Microsoft.AspNetCore.Authorization;

namespace Budgeteer.Server.AuthenticationAuthorization;

public class AuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>
{
    private readonly UnitOfWork _unitOfWork;

    public AuthorizationHandler(
        UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
    {
        if (context.User.TryGetEmail(out var email) && await _unitOfWork.Users.AnyAsync(user => user.Email == email))
            context.Succeed(requirement);
    }
}

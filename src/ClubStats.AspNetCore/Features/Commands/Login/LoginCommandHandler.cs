using ClubStats.AspNetCore.DataAccess.Entities;
using ClubStats.AspNetCore.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClubStats.AspNetCore.Features.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<StatusCodeResult, ApiError>>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public LoginCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<Result<StatusCodeResult, ApiError>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Login.Username);
        var invalidCredentialsError = new ApiError(StatusCodes.Status401Unauthorized, "Invalid credentials");
        
        if (user == null)
        {
            return Result<StatusCodeResult, ApiError>.Error(invalidCredentialsError);
        }

        var result = await _signInManager.PasswordSignInAsync(user, request.Login.Password, request.Login.Persistent, true);
        
        if (result.Succeeded)
        {
            return Result<StatusCodeResult, ApiError>.Ok(new OkResult());
        }

        if (result.RequiresTwoFactor)
        {
            var notSupportedError = new ApiError(StatusCodes.Status501NotImplemented, "Two factor authentication is not supported");

            return Result<StatusCodeResult, ApiError>.Error(notSupportedError);
        }

        if (result.IsLockedOut)
        {
            var lockedOutError = new ApiError(StatusCodes.Status401Unauthorized, "Too many failed login attempts. Try again later.");

            return Result<StatusCodeResult, ApiError>.Error(lockedOutError);
        }

        if (result.IsNotAllowed)
        {
            var notVerifiedError = new ApiError(StatusCodes.Status401Unauthorized, "Your account is unverified. Check your email.");

            return Result<StatusCodeResult, ApiError>.Error(notVerifiedError);
        }

        return Result<StatusCodeResult, ApiError>.Error(invalidCredentialsError);
    }
}
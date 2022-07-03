using ClubStats.AspNetCore.DataAccess.Entities;
using ClubStats.AspNetCore.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ClubStats.AspNetCore.Features.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result<ConfirmEmailResponse, ApiError>>
{
    private readonly UserManager<User> _userManager;

    public ConfirmEmailCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<Result<ConfirmEmailResponse, ApiError>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.ConfirmEmail.UserId);
        
        if (user is null)
        {
            var notFoundError = new ApiError(StatusCodes.Status404NotFound, "No registered account was found");
            
            return Result<ConfirmEmailResponse, ApiError>.Error(notFoundError);
        }

        if (await _userManager.IsEmailConfirmedAsync(user))
        {
            var confirmedError = new ApiError(StatusCodes.Status409Conflict, "Email was already confirmed");

            return Result<ConfirmEmailResponse, ApiError>.Error(confirmedError);
        }

        var result = await _userManager.ConfirmEmailAsync(user, request.ConfirmEmail.ConfirmationId);

        if (result.Succeeded)
        {
            var response = new ConfirmEmailResponse { Success = true, EmailAddress = user.Email };

            return Result<ConfirmEmailResponse, ApiError>.Ok(response);
        }
        
        var error = new ApiError(StatusCodes.Status400BadRequest, "Could not confirm email address");
            
        return Result<ConfirmEmailResponse, ApiError>.Error(error); 
    }
}
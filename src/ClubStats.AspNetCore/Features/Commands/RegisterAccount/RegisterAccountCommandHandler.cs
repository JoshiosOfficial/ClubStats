using ClubStats.AspNetCore.DataAccess.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ClubStats.AspNetCore.Features.Commands.RegisterAccount;

public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, IdentityResult>
{
    private readonly UserManager<User> _userManager;

    public RegisterAccountCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        var body = request.AccountDetails;
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = body.Username,
            Email = body.EmailAddress,
            FirstName = body.FirstName,
            LastName = body.LastName
        };

        return await _userManager.CreateAsync(user, body.Password);
    }
}
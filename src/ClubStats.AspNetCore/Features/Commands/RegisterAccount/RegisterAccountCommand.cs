using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ClubStats.AspNetCore.Features.Commands.RegisterAccount;

public class RegisterAccountCommand : IRequest<IdentityResult>
{ 
    public RegisterAccount AccountDetails { get; set; }
}
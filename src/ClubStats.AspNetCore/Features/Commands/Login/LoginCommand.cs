using ClubStats.AspNetCore.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClubStats.AspNetCore.Features.Commands.Login;

public class LoginCommand : IRequest<Result<StatusCodeResult, ApiError>>
{
    public Login Login { get; set; }
}
using ClubStats.AspNetCore.Utilities;
using MediatR;

namespace ClubStats.AspNetCore.Features.Commands.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<Result<ConfirmEmailResponse, ApiError>>
{
    public ConfirmEmail ConfirmEmail { get; set; }
}
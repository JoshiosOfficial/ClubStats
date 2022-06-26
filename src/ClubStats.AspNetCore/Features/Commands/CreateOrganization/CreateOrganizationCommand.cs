using ClubStats.AspNetCore.Utilities;
using MediatR;

namespace ClubStats.AspNetCore.Features.Commands.CreateOrganization;

public class CreateOrganizationCommand : IRequest<Result<Guid, ApiError>>
{
    public CreateOrganization Organization { get; set; }
}

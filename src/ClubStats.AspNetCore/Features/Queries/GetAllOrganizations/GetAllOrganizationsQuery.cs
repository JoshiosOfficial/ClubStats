using ClubStats.AspNetCore.Utilities;
using MediatR;

namespace ClubStats.AspNetCore.Features.Queries.GetAllOrganizations;

public class GetAllOrganizationsQuery : IRequest<Result<List<GetOrganizationResponse>, ApiError>>
{
}
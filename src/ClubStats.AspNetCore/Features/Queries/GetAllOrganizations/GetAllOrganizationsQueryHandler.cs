using ClubStats.AspNetCore.DataAccess;
using ClubStats.AspNetCore.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClubStats.AspNetCore.Features.Queries.GetAllOrganizations;

public class GetAllOrganizationsQueryHandler : IRequestHandler<GetAllOrganizationsQuery, Result<List<GetOrganizationResponse>, ApiError>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAllOrganizationsQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<GetOrganizationResponse>, ApiError>> Handle(GetAllOrganizationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var organizations = await _dbContext.Organizations
                .Select(organization => new GetOrganizationResponse { Id = organization.Id, Name = organization.Name })
                .ToListAsync(cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result<List<GetOrganizationResponse>, ApiError>.Ok(organizations);
        }
        catch
        {
            var error = new ApiError(500, "Could not fetch organizations from database");

            return Result<List<GetOrganizationResponse>, ApiError>.Error(error);
        }
    }
}
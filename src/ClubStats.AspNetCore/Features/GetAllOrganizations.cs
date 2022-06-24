using ClubStats.AspNetCore.DataAccess;
using ClubStats.AspNetCore.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClubStats.AspNetCore.Features;

public class GetOrganization
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class GetAllOrganizationsCommand : IRequest<Result<List<GetOrganization>, ApiError>>
{
}

public class GetAllOrganizationsCommandHandler : IRequestHandler<GetAllOrganizationsCommand, Result<List<GetOrganization>, ApiError>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAllOrganizationsCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<GetOrganization>, ApiError>> Handle(GetAllOrganizationsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var organizations = await _dbContext.Organizations
                .Select(organization => new GetOrganization { Id = organization.Id, Name = organization.Name })
                .ToListAsync(cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result<List<GetOrganization>, ApiError>.Ok(organizations);
        }
        catch
        {
            var error = new ApiError(500, "Could not fetch organizations from database");

            return Result<List<GetOrganization>, ApiError>.Error(error);
        }
    }
}
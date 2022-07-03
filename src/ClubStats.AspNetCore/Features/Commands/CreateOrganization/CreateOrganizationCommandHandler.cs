using ClubStats.AspNetCore.DataAccess;
using ClubStats.AspNetCore.DataAccess.Entities;
using ClubStats.AspNetCore.Utilities;
using MediatR;

namespace ClubStats.AspNetCore.Features.Commands.CreateOrganization;

public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, Result<Guid, ApiError>>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateOrganizationCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, ApiError>> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organizationRequest = request.Organization;
        
        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            Name = organizationRequest.Name
        };

        try
        {
            _dbContext.Organizations.Add(organization);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<Guid, ApiError>.Ok(organization.Id);
        }
        catch
        {
            var error = new ApiError(StatusCodes.Status500InternalServerError, "Could not add organization to database");

            return Result<Guid, ApiError>.Error(error);
        }
    }
}
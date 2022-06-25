﻿using System.ComponentModel.DataAnnotations;
using ClubStats.AspNetCore.DataAccess;
using ClubStats.AspNetCore.DataAccess.Entities;
using ClubStats.AspNetCore.Utilities;
using Mapster;
using MediatR;

namespace ClubStats.AspNetCore.Features;

public class CreateOrganization
{ 
    [Required] 
    public string Name { get; set; } = string.Empty;
}

public class CreateOrganizationCommand : IRequest<Result<Guid, ApiError>>
{
    public CreateOrganization Organization { get; set; }
}

public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, Result<Guid, ApiError>>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateOrganizationCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, ApiError>> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = request.Organization.Adapt<Organization>();

        organization.Id = new Guid();

        try
        {
            _dbContext.Organizations.Add(organization);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<Guid, ApiError>.Ok(organization.Id);
        }
        catch
        {
            var error = new ApiError(500, "Could not add organization to database");

            return Result<Guid, ApiError>.Error(error);
        }
    }
}
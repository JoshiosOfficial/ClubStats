using ClubStats.AspNetCore.Features.Commands.CreateOrganization;
using ClubStats.AspNetCore.Features.Queries.GetAllOrganizations;
using ClubStats.AspNetCore.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClubStats.AspNetCore.Controllers;

[ApiController]
[Route("api/organizations")]
public class OrganizationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrganizationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProblemDetails]
    public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganization createOrganization)
    {
        var command = new CreateOrganizationCommand { Organization = createOrganization };
        var response = await _mediator.Send(command);

        return response.Match(
            guid => Ok(new { guid }),
            error => error.Result()
        );
    }

    [HttpGet]
    [ProblemDetails]
    public async Task<IActionResult> GetAllOrganizations()
    {
        var query = new GetAllOrganizationsQuery();
        var response = await _mediator.Send(query);

        return response.Match(
            Ok, error => error.Result()
        );
    }
}
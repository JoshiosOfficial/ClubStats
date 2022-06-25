using ClubStats.AspNetCore.Features;
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
        var createMeetingCommand = new CreateOrganizationCommand { Organization = createOrganization };
        var response = await _mediator.Send(createMeetingCommand);

        return response.Match(
            guid => Ok(new { guid }),
            error => StatusCode(error.Code, error)
        );
    }

    [HttpGet]
    [ProblemDetails]
    public async Task<IActionResult> GetAllOrganizations()
    {
        var getAllOrganizationsQuery = new GetAllOrganizationsQuery();
        var response = await _mediator.Send(getAllOrganizationsQuery);

        return response.Match(
            Ok,
            error => StatusCode(error.Code, error)
        );
    }
}
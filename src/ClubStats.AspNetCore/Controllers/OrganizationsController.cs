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
    public IActionResult CreateOrganization([FromBody] CreateOrganization createOrganization)
    {
        var createMeetingCommand = new CreateOrganizationCommand { Organization = createOrganization };
        var response = _mediator.Send(createMeetingCommand);

        return response.Result.Match(
            guid => Ok(new { guid }),
            error => StatusCode(error.Code, error)
        );
    }

    [HttpGet]
    [ProblemDetails]
    public IActionResult GetAllOrganizations()
    {
        var getAllOrganizationsCommand = new GetAllOrganizationsCommand();
        var response = _mediator.Send(getAllOrganizationsCommand);

        return response.Result.Match(
            Ok,
            error => StatusCode(error.Code, error)
        );
    }
}
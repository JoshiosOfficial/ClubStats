using ClubStats.AspNetCore.Features;
using ClubStats.AspNetCore.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClubStats.AspNetCore.Controllers;

[ApiController]
[Route("api/meetings")]
public class MeetingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MeetingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProblemDetails]
    public IActionResult CreateMeeting([FromBody] CreateMeeting createMeeting)
    {
        var createMeetingCommand = new CreateMeetingCommand { Meeting = createMeeting };
        var response = _mediator.Send(createMeetingCommand);

        return response.Result.Match(
            guid => Ok(new { guid }),
            error => StatusCode(500, error)
        );
    }
}
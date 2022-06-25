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
    public async Task<IActionResult> CreateMeeting([FromBody] CreateMeeting createMeeting)
    {
        var createMeetingCommand = new CreateMeetingCommand { Meeting = createMeeting };
        var response = await _mediator.Send(createMeetingCommand);

        return response.Match(
            guid => Ok(new { guid }),
            error => StatusCode(error.Code, error)
        );
    }

    [HttpGet]
    [ProblemDetails]
    public async Task<IActionResult> GetMeetings([FromQuery] GetAllMeetings getAllMeetings)
    {
        var getAllMeetingsCommand = new GetAllMeetingsCommand { MeetingQuery = getAllMeetings };
        var response = await _mediator.Send(getAllMeetingsCommand);

        return response.Match(
            Ok,
            error => StatusCode(error.Code, error)
        );
    }
} 

using System.ComponentModel.DataAnnotations;
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
            error => error.Result()
        );
    }

    [HttpGet]
    [ProblemDetails]
    public async Task<IActionResult> GetAllMeetings([FromQuery] GetAllMeetings getAllMeetings)
    {
        var getAllMeetingsQuery = new GetAllMeetingsQuery { MeetingQuery = getAllMeetings };
        var response = await _mediator.Send(getAllMeetingsQuery);

        return response.Match(
            Ok,
            error => error.Result()
        );
    }

    [HttpGet("{id:guid}")]
    [ProblemDetails]
    public async Task<IActionResult> GetMeeting(Guid id)
    {
        var getMeetingQuery = new GetMeetingQuery { Id = id };
        var response = await _mediator.Send(getMeetingQuery);

        return response.Match(
            Ok,
            error => error.Result()
        );
    }
} 

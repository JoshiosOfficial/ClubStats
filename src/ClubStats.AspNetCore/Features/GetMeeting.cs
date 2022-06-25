using System.ComponentModel.DataAnnotations;
using ClubStats.AspNetCore.DataAccess;
using ClubStats.AspNetCore.DataAccess.Entities;
using ClubStats.AspNetCore.Utilities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClubStats.AspNetCore.Features;

public class GetMeeting
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string State { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Attendee> Attendees { get; set; } = new();   
}

public class GetMeetingQuery : IRequest<Result<GetMeeting, ApiError>>
{
    [Required]
    public Guid Id { get; set; }
}

public class GetMeetingQueryHandler : IRequestHandler<GetMeetingQuery, Result<GetMeeting, ApiError>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetMeetingQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<GetMeeting, ApiError>> Handle(GetMeetingQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var meeting = await _dbContext.Meetings
                .Where(meeting => meeting.Id == request.Id)
                .Include(meeting => meeting.Attendees)
                .Select(meeting => meeting.Adapt<GetMeeting>())
                .FirstOrDefaultAsync(cancellationToken);
            
            if (meeting == null)
            {
                var error = new ApiError(404, "Could not find meeting with specified id");

                return Result<GetMeeting, ApiError>.Error(error);
            }

            meeting.State = GetStateFilter(meeting).ToString();
            
            return Result<GetMeeting, ApiError>.Ok(meeting);
        }
        catch
        {
            var error = new ApiError(500, "Could not fetch meeting from database");

            return Result<GetMeeting, ApiError>.Error(error);
        }
    }
    
    private MeetingState GetStateFilter(GetMeeting meeting)
    {
        var now = DateTime.UtcNow;
        
        if (meeting.StartDate < now && meeting.EndDate > now)
        {
            return MeetingState.Ongoing;
        }

        if (meeting.StartDate > now)
        {
            return MeetingState.Scheduled;
        }

        if (meeting.EndDate < now)
        {
            return MeetingState.Past;
        }

        return MeetingState.Any;
    }
}
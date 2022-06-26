using ClubStats.AspNetCore.DataAccess;
using ClubStats.AspNetCore.Features.Queries.GetAllMeetings;
using ClubStats.AspNetCore.Utilities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClubStats.AspNetCore.Features.Queries.GetMeeting;

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
            
            if (meeting is null)
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
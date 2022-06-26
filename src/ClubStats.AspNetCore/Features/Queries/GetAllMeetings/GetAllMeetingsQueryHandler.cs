using System.Linq.Expressions;
using ClubStats.AspNetCore.DataAccess;
using ClubStats.AspNetCore.DataAccess.Entities;
using ClubStats.AspNetCore.Utilities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClubStats.AspNetCore.Features.Queries.GetAllMeetings;

public class GetAllMeetingsQueryHandler : IRequestHandler<GetAllMeetingsQuery, Result<List<MeetingResponse>, ApiError>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAllMeetingsQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<MeetingResponse>, ApiError>> Handle(GetAllMeetingsQuery request, CancellationToken cancellationToken)
    {
        var meetingQuery = request.MeetingQuery;

        try
        {
            var queryable = _dbContext.Meetings
                .Where(meeting => meeting.OrganizationId == meetingQuery.OrganizationId)
                .Where(GetStateFilter(meetingQuery.State));
            
            if (!String.IsNullOrWhiteSpace(meetingQuery.Description))
            {
                queryable = queryable.Where(meeting => meeting.Description.ToLower().Contains(meetingQuery.Description.ToLower()));
            }

            var results = await queryable
                .Select(meeting => meeting.Adapt<MeetingResponse>())
                .ToListAsync(cancellationToken);
            
            return Result<List<MeetingResponse>, ApiError>.Ok(results);
        }
        catch
        {
            var error = new ApiError(500, "Could not retrieve meetings from database.");

            return Result<List<MeetingResponse>, ApiError>.Error(error);
        }
    }

    private Expression<Func<Meeting, bool>> GetStateFilter(MeetingState meetingState)
    {
        return meetingState switch
        {
            MeetingState.Ongoing => meeting => meeting.StartDate < DateTime.UtcNow && meeting.EndDate > DateTime.UtcNow,
            MeetingState.Scheduled => meeting => meeting.StartDate > DateTime.UtcNow,
            MeetingState.Past => meeting => meeting.EndDate < DateTime.UtcNow,
            _ => _ => true
        };
    }
}
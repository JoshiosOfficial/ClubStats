using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using ClubStats.AspNetCore.DataAccess;
using ClubStats.AspNetCore.DataAccess.Entities;
using ClubStats.AspNetCore.Utilities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClubStats.AspNetCore.Features;

public enum MeetingState
{
    Any,
    Ongoing,
    Scheduled,
    Past
}

public class GetAllMeetings
{
    [Required]
    public Guid OrganizationId { get; set; }

    public MeetingState State { get; set; } = MeetingState.Any;
    public string? Description { get; set; }
}

public class MeetingResponse
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class GetAllMeetingsCommand : IRequest<Result<List<MeetingResponse>, ApiError>>
{
    public GetAllMeetings MeetingQuery { get; set; }
}

public class GetAllMeetingsCommandHandler : IRequestHandler<GetAllMeetingsCommand, Result<List<MeetingResponse>, ApiError>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAllMeetingsCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<MeetingResponse>, ApiError>> Handle(GetAllMeetingsCommand request, CancellationToken cancellationToken)
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
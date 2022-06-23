using ClubStats.AspNetCore.DataAccess;
using ClubStats.AspNetCore.DataAccess.Entities;
using ClubStats.AspNetCore.Utilities;
using Mapster;
using MediatR;

namespace ClubStats.AspNetCore.Features;

public class CreateMeeting
{
    public Guid OrganizationId { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
public class CreateMeetingCommand : IRequest<Result<Guid, ApiError>>
{
    public CreateMeeting Meeting { get; set; }
}

public class CreateMeetingCommandHandler : IRequestHandler<CreateMeetingCommand, Result<Guid, ApiError>>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateMeetingCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, ApiError>> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
    {
        var meeting = request.Meeting.Adapt<Meeting>();

        meeting.Id = new Guid();
        meeting.Attendees = new List<Attendee>();

        try
        {
            _dbContext.Add(meeting);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<Guid, ApiError>.Ok(meeting.Id);
        }
        catch
        {
            // TODO Handle the exception better
            var error = new ApiError(500, "Could not add meeting to database");

            return Result<Guid, ApiError>.Error(error);
        }
    }
}
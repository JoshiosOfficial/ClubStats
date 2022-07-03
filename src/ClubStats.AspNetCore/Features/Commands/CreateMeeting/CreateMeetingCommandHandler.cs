using ClubStats.AspNetCore.DataAccess;
using ClubStats.AspNetCore.DataAccess.Entities;
using ClubStats.AspNetCore.Utilities;
using MediatR;

namespace ClubStats.AspNetCore.Features.Commands.CreateMeeting;

public class CreateMeetingCommandHandler : IRequestHandler<CreateMeetingCommand, Result<Guid, ApiError>>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateMeetingCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, ApiError>> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
    {
        var meetingRequest = request.Meeting;
        
        try
        {
            var organization = await _dbContext.Organizations.FindAsync(meetingRequest.OrganizationId);

            if (organization is null)
            {
                var error = new ApiError(StatusCodes.Status404NotFound, "Invalid organization id was provided.");

                return Result<Guid, ApiError>.Error(error);
            }

            var meeting = new Meeting
            {
                Id = Guid.NewGuid(),
                Organization = organization,
                OrganizationId = organization.Id,
                Description = meetingRequest.Description,
                StartDate = meetingRequest.StartDate,
                EndDate = meetingRequest.EndDate
            };
            
            organization.Meetings.Add(meeting);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<Guid, ApiError>.Ok(meeting.Id);
        }
        catch
        {
            // TODO Handle the exception better
            var error = new ApiError(StatusCodes.Status500InternalServerError, "Could not add meeting to database");

            return Result<Guid, ApiError>.Error(error);
        }
    }
}
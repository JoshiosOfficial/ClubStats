using System.ComponentModel.DataAnnotations;
using ClubStats.AspNetCore.DataAccess;
using ClubStats.AspNetCore.DataAccess.Entities;
using ClubStats.AspNetCore.Utilities;
using FluentValidation;
using Mapster;
using MediatR;

namespace ClubStats.AspNetCore.Features;

public class CreateMeeting
{
    public Guid OrganizationId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class CreateMeetingValidator : AbstractValidator<CreateMeeting>
{
    public CreateMeetingValidator()
    {
        RuleFor(m => m.OrganizationId)
            .NotNull();

        RuleFor(m => m.Description)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(2000);

        RuleFor(m => m)
            .Must(m => m.StartDate < DateTime.UtcNow)
            .WithName("StartDate")
            .WithMessage("Start date time must be upcoming.");

        RuleFor(m => m)
            .Must(m => m.StartDate > m.EndDate)
            .WithName("EndDate")
            .WithMessage("End date time must be after start date time.");

        RuleFor(m => m.StartDate)
            .NotNull();

        RuleFor(m => m.EndDate)
            .NotNull();
    }
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
        var meetingRequest = request.Meeting;
        
        try
        {
            var organization = await _dbContext.Organizations.FindAsync(meetingRequest.OrganizationId);

            if (organization is null)
            {
                var error = new ApiError(404, "Invalid organization id was provided.");

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
            var error = new ApiError(500, "Could not add meeting to database");

            return Result<Guid, ApiError>.Error(error);
        }
    }
}
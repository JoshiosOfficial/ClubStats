using System.ComponentModel.DataAnnotations;
using ClubStats.AspNetCore.DataAccess;
using ClubStats.AspNetCore.DataAccess.Entities;
using ClubStats.AspNetCore.Utilities;
using Mapster;
using MediatR;

namespace ClubStats.AspNetCore.Features;

public class CreateMeeting : IValidatableObject
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    [StringLength(2000, MinimumLength = 5)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var now = DateTime.UtcNow;

        if (StartDate < now)
        {
            yield return new ValidationResult(
                "Start date time must be upcoming.",
                new[] { nameof(StartDate) });
        }
        
        if (StartDate < EndDate)
        {
            yield return new ValidationResult(
                "End date time must be after start date time.",
                new[] { nameof(EndDate) });
        }
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
        var meeting = request.Meeting.Adapt<Meeting>();

        meeting.Id = new Guid();

        try
        {
            var organization = await _dbContext.Organizations.FindAsync(request.Meeting.OrganizationId);

            if (organization == null)
            {
                var error = new ApiError(400, "Invalid organization id was provided.");

                return Result<Guid, ApiError>.Error(error);
            }

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
using FluentValidation;

namespace ClubStats.AspNetCore.Features.Commands.CreateMeeting;

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
            .Must(m => m.StartDate > DateTime.UtcNow)
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
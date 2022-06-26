using System.ComponentModel.DataAnnotations;

namespace ClubStats.AspNetCore.Features.Queries.GetAllMeetings;

public class GetAllMeetings
{
    [Required]
    public Guid OrganizationId { get; set; }

    public MeetingState State { get; set; } = MeetingState.Any;
    public string? Description { get; set; }
}
namespace ClubStats.AspNetCore.Features.Commands.CreateMeeting;

public class CreateMeeting
{
    public Guid OrganizationId { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

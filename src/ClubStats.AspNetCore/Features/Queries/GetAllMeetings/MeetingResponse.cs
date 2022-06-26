namespace ClubStats.AspNetCore.Features.Queries.GetAllMeetings;

public class MeetingResponse
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

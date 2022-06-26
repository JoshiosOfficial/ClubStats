using ClubStats.AspNetCore.DataAccess.Entities;

namespace ClubStats.AspNetCore.Features.Queries.GetMeeting;

public class GetMeeting
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string State { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Attendee> Attendees { get; set; } = new();   
}
namespace ClubStats.AspNetCore.DataAccess.Entities;

public class Meeting
{
    public Guid Id { get; set; }
    public Organization Organization { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Attendee> Attendees { get; set; } = new();
}
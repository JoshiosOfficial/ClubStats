namespace ClubStats.AspNetCore.DataAccess.Entities;

public class Attendee
{
    public Guid Id { get; set; }
    public Meeting Meeting { get; set; }
    public OrganizationMember Member { get; set; }
    public DateTime SignedIn { get; set; }
    public DateTime SignedOut { get; set; }
}
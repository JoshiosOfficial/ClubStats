namespace ClubStats.AspNetCore.DataAccess.Entities;

public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Meeting> Meetings { get; set; }
    public List<Group> Groups { get; set; }
    public List<OrganizationMember> Members { get; set; }
}
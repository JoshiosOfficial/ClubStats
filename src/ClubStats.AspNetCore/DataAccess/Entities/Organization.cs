namespace ClubStats.AspNetCore.DataAccess.Entities;

public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Meeting> Meetings { get; set; } = new();
    public List<Group> Groups { get; set; } = new();
    public List<OrganizationMember> Members { get; set; } = new();
}
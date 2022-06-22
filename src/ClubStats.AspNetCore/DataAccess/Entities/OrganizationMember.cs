namespace ClubStats.AspNetCore.DataAccess.Entities;

public class OrganizationMember
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public Organization Organization { get; set; }
    public PermissionLevel PermissionLevel { get; set; }
    public List<Group> Groups { get; set; }
}
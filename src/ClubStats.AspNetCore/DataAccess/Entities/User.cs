namespace ClubStats.AspNetCore.DataAccess.Entities;

public class User
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<OrganizationMember> OrganizationMembers { get; set; } = new();
}
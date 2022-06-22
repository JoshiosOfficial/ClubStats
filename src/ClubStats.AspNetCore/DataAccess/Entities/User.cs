namespace ClubStats.AspNetCore.DataAccess.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public bool EmailConfirmed { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public List<OrganizationMember> OrganizationMembers { get; set; }
}
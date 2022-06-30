using Microsoft.AspNetCore.Identity;

namespace ClubStats.AspNetCore.DataAccess.Entities;

public class User : IdentityUser<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<OrganizationMember> OrganizationMembers { get; set; } = new();
}
namespace ClubStats.AspNetCore.DataAccess.Entities;

public class Group
{
    public Organization Organization { get; set; }
    public List<OrganizationMember> MemberHolders { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
}
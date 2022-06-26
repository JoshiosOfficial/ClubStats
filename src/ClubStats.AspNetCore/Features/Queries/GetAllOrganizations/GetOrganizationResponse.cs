namespace ClubStats.AspNetCore.Features.Queries.GetAllOrganizations;

public class GetOrganizationResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
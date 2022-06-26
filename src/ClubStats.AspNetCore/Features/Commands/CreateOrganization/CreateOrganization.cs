using System.ComponentModel.DataAnnotations;

namespace ClubStats.AspNetCore.Features.Commands.CreateOrganization;

public class CreateOrganization
{ 
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Name { get; set; } = string.Empty;
}

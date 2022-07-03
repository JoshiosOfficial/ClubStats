using System.ComponentModel.DataAnnotations;

namespace ClubStats.AspNetCore.Features.Commands.ConfirmEmail;

public class ConfirmEmail
{
    [Required]
    public string UserId { get; set; }
    [Required]
    public string ConfirmationId { get; set; }
}

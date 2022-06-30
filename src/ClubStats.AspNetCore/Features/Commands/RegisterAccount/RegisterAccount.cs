using System.ComponentModel.DataAnnotations;

namespace ClubStats.AspNetCore.Features.Commands.RegisterAccount;

public class RegisterAccount
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string EmailAddress { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    public string Password { get; set; }
}
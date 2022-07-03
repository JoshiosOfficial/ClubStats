using System.ComponentModel.DataAnnotations;

namespace ClubStats.AspNetCore.Features.Commands.Login;

public class Login
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }

    public bool Persistent { get; set; } = false;
}
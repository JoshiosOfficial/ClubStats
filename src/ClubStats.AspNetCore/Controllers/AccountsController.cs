using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using ClubStats.AspNetCore.DataAccess;
using ClubStats.AspNetCore.DataAccess.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClubStats.AspNetCore.Controllers;

[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _dbContext;

    public AccountsController(IConfiguration configuration, ApplicationDbContext dbContext)
    {
        _configuration = configuration;
        _dbContext = dbContext;
    }
    
    [HttpGet("accounts/login")]
    public async Task Login()
    {
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri("accounts/login/callback")
            .WithAudience(_configuration["Auth0:Audience"])
            .Build();

        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        
        Console.WriteLine(User.Identity?.IsAuthenticated == true);
        if (User?.Identity?.IsAuthenticated == true)
        {
            var findFirst = User.FindFirst(ClaimTypes.NameIdentifier);
            Console.WriteLine(findFirst?.Value);
        }
    }

    [HttpGet("accounts/login/callback")]
    public async Task<IActionResult> LoginGetCallback()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)!;

            if (await _dbContext.Users.AnyAsync(u => u.Id == id.Value))
            {
                return Ok();
            }

            var entity = new User
            {
                Id = id.Value,
                FirstName = "",
                LastName = ""
            };
            
            _dbContext.Users.Add(entity);
            await _dbContext.SaveChangesAsync();
        }
        
        return Ok();
    }

    [Authorize]
    [HttpGet("accounts/logout")]
    public async Task Logout()
    {
        var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri("/")
            .Build();

        await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
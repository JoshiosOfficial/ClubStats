using ClubStats.AspNetCore.Features.Commands.RegisterAccount;
using ClubStats.AspNetCore.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClubStats.AspNetCore.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProblemDetails]
    public async Task<IActionResult> Register([FromBody] RegisterAccount registerAccount)
    {
        var command = new RegisterAccountCommand { AccountDetails = registerAccount };
        var response = await _mediator.Send(command);
        
        if (response.Succeeded)
        {
            return Ok();
        }
        
        foreach (var error in response.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(new ValidationProblemDetails(ModelState));
    }
}
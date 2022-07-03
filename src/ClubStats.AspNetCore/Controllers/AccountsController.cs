using ClubStats.AspNetCore.Features.Commands.ConfirmEmail;
using ClubStats.AspNetCore.Features.Commands.Login;
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

    [HttpPost("login")]
    [ProblemDetails]
    public async Task<IActionResult> Login([FromBody] Login login)
    {
        var command = new LoginCommand { Login = login };
        var response = await _mediator.Send(command);
        
        return response.Match(result => result, error => error.Result());
    }

    [HttpPost("confirm")]
    [ProblemDetails]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmail confirmEmail)
    {
        var command = new ConfirmEmailCommand { ConfirmEmail = confirmEmail };
        var response = await _mediator.Send(command);

        return response.Match(Ok, error => error.Result());
    }
}

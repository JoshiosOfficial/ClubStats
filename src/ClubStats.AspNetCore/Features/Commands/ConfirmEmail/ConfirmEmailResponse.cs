namespace ClubStats.AspNetCore.Features.Commands.ConfirmEmail;

public class ConfirmEmailResponse
{
    public bool Success { get; set; }
    public string EmailAddress { get; set; }
}
using System.ComponentModel.DataAnnotations;
using ClubStats.AspNetCore.Utilities;
using MediatR;

namespace ClubStats.AspNetCore.Features.Queries.GetMeeting;

public class GetMeetingQuery : IRequest<Result<GetMeeting, ApiError>>
{
    [Required]
    public Guid Id { get; set; }
}

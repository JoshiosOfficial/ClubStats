using ClubStats.AspNetCore.Utilities;
using MediatR;

namespace ClubStats.AspNetCore.Features.Commands.CreateMeeting;

public class CreateMeetingCommand : IRequest<Result<Guid, ApiError>>
{ 
    public CreateMeeting Meeting { get; set; }
}
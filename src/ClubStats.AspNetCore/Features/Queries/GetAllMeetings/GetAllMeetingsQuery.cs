using ClubStats.AspNetCore.Utilities;
using MediatR;

namespace ClubStats.AspNetCore.Features.Queries.GetAllMeetings;

public class GetAllMeetingsQuery : IRequest<Result<List<MeetingResponse>, ApiError>>
{
    public GetAllMeetings MeetingQuery { get; set; }
}
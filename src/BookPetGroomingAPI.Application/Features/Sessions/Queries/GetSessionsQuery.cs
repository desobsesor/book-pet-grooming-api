using MediatR;

namespace BookPetGroomingAPI.Application.Features.Sessions.Queries
{
    /// <summary>
    /// Query to retrieve all sessions
    /// </summary>
    public class GetSessionsQuery : IRequest<List<SessionDto>>
    {
    }
}
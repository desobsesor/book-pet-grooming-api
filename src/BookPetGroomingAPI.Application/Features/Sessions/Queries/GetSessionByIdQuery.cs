using MediatR;

namespace BookPetGroomingAPI.Application.Features.Sessions.Queries
{
    /// <summary>
    /// Query to retrieve a session by its ID
    /// </summary>
    /// <remarks>
    /// Constructor for GetSessionByIdQuery
    /// </remarks>
    /// <param name="sessionId">The ID of the session to retrieve</param>
    public class GetSessionByIdQuery(int sessionId) : IRequest<SessionDto>
    {
        /// <summary>
        /// The ID of the session to retrieve
        /// </summary>
        public int SessionId { get; } = sessionId;
    }
}
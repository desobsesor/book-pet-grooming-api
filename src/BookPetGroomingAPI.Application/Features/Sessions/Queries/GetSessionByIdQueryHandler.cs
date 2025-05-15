using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Sessions.Queries
{
    /// <summary>
    /// Handler for processing the GetSessionByIdQuery and returning a specific session
    /// </summary>
    /// <remarks>
    /// Constructor for GetSessionByIdQueryHandler
    /// </remarks>
    /// <param name="sessionRepository">Repository for session operations</param>
    /// <param name="mapper">Mapping service</param>
    public class GetSessionByIdQueryHandler(ISessionRepository sessionRepository, IMapper mapper) : IRequestHandler<GetSessionByIdQuery, SessionDto>
    {

        /// <summary>
        /// Handles the query to retrieve a session by its ID
        /// </summary>
        /// <param name="request">The GetSessionByIdQuery with the session ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Session DTO if found, otherwise throws an exception</returns>
        public async Task<SessionDto> Handle(GetSessionByIdQuery request, CancellationToken cancellationToken)
        {
            var session = await sessionRepository.GetByIdAsync(request.SessionId);

            if (session == null)
            {
                throw new KeyNotFoundException($"Session with ID {request.SessionId} not found");
            }

            return mapper.Map<SessionDto>(session);
        }
    }
}
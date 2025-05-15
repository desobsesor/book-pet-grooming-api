using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Sessions.Queries
{
    /// <summary>
    /// Handler for processing the GetSessionsQuery and returning the list of sessions
    /// </summary>
    /// <remarks>
    /// Constructor for GetSessionsQueryHandler
    /// </remarks>
    /// <param name="sessionRepository">Repository for session operations</param>
    /// <param name="mapper">Mapping service</param>
    public class GetSessionsQueryHandler(ISessionRepository sessionRepository, IMapper mapper) : IRequestHandler<GetSessionsQuery, List<SessionDto>>
    {

        /// <summary>
        /// Handles the query to retrieve all sessions
        /// </summary>
        /// <param name="request">The GetSessionsQuery</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of session DTOs</returns>
        public async Task<List<SessionDto>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
        {
            var sessions = await sessionRepository.GetAllAsync();
            return mapper.Map<List<SessionDto>>(sessions);
        }
    }
}
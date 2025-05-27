using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Groomers.Queries
{
    /// <summary>
    /// Handler for processing the GetGroomersQuery and returning the list of groomers
    /// </summary>
    /// <remarks>
    /// Constructor for GetGroomersQueryHandler
    /// </remarks>
    /// <param name="groomerRepository">Repository for groomer operations</param>
    /// <param name="mapper">Mapping service</param>
    public class GetGroomersQueryHandler(IGroomerRepository groomerRepository, IMapper mapper) : IRequestHandler<GetGroomersQuery, List<GroomerDto>>
    {

        /// <summary>
        /// Handles the query to retrieve all groomers
        /// </summary>
        /// <param name="request">The GetGroomersQuery</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of groomer DTOs</returns>
        public async Task<List<GroomerDto>> Handle(GetGroomersQuery request, CancellationToken cancellationToken)
        {
            var groomers = await groomerRepository.GetAllAsync();
            return mapper.Map<List<GroomerDto>>(groomers);
        }
    }
}
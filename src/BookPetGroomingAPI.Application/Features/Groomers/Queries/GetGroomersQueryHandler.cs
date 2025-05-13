using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Groomers.Queries
{
    /// <summary>
    /// Handler for processing the GetGroomersQuery and returning the list of groomers
    /// </summary>
    public class GetGroomersQueryHandler : IRequestHandler<GetGroomersQuery, List<GroomerDto>>
    {
        private readonly IGroomerRepository _groomerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for GetGroomersQueryHandler
        /// </summary>
        /// <param name="groomerRepository">Repository for groomer operations</param>
        /// <param name="mapper">Mapping service</param>
        public GetGroomersQueryHandler(IGroomerRepository groomerRepository, IMapper mapper)
        {
            _groomerRepository = groomerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the query to retrieve all groomers
        /// </summary>
        /// <param name="request">The GetGroomersQuery</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of groomer DTOs</returns>
        public async Task<List<GroomerDto>> Handle(GetGroomersQuery request, CancellationToken cancellationToken)
        {
            var groomers = await _groomerRepository.GetAllAsync();
            return _mapper.Map<List<GroomerDto>>(groomers);
        }
    }
}
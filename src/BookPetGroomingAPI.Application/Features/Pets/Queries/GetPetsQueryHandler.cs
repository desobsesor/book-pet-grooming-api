using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Pets.Queries
{
    /// <summary>
    /// Handler for processing the GetPetsQuery and returning the list of pets
    /// </summary>
    public class GetPetsQueryHandler : IRequestHandler<GetPetsQuery, List<PetDto>>
    {
        private readonly IPetRepository _petRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for GetPetsQueryHandler
        /// </summary>
        /// <param name="petRepository">Repository for pet operations</param>
        /// <param name="mapper">Mapping service</param>
        public GetPetsQueryHandler(IPetRepository petRepository, IMapper mapper)
        {
            _petRepository = petRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the query to retrieve all pets
        /// </summary>
        /// <param name="request">The GetPetsQuery</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of pet DTOs</returns>
        public async Task<List<PetDto>> Handle(GetPetsQuery request, CancellationToken cancellationToken)
        {
            var pets = await _petRepository.GetAllAsync();
            return _mapper.Map<List<PetDto>>(pets);
        }
    }
}
using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Pets.Queries
{
    /// <summary>
    /// Handler for processing the GetPetsQuery and returning the list of pets
    /// </summary>
    /// <remarks>
    /// Constructor for GetPetsQueryHandler
    /// </remarks>
    /// <param name="petRepository">Repository for pet operations</param>
    /// <param name="mapper">Mapping service</param>
    public class GetPetsQueryHandler(IPetRepository petRepository, IMapper mapper) : IRequestHandler<GetPetsQuery, List<PetDto>>
    {

        /// <summary>
        /// Handles the query to retrieve all pets
        /// </summary>
        /// <param name="request">The GetPetsQuery</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of pet DTOs</returns>
        public async Task<List<PetDto>> Handle(GetPetsQuery request, CancellationToken cancellationToken)
        {
            var pets = await petRepository.GetAllAsync();
            return mapper.Map<List<PetDto>>(pets);
        }
    }
}
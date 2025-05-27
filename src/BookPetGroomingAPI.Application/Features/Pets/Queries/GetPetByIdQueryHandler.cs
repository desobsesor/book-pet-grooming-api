using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Pets.Queries
{
    /// <summary>
    /// Handler for processing the GetPetByIdQuery and retrieving a pet by ID
    /// </summary>
    /// <remarks>
    /// Constructor for GetPetByIdQueryHandler
    /// </remarks>
    /// <param name="petRepository">Repository for pet operations</param>
    /// <param name="mapper">Mapping service</param>
    public class GetPetByIdQueryHandler(IPetRepository petRepository, IMapper mapper) : IRequestHandler<GetPetByIdQuery, PetDto>
    {

        /// <summary>
        /// Handles the query to retrieve a pet by ID
        /// </summary>
        /// <param name="request">The GetPetByIdQuery with pet ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The pet DTO if found</returns>
        public async Task<PetDto> Handle(GetPetByIdQuery request, CancellationToken cancellationToken)
        {
            var pet = await petRepository.GetByIdAsync(request.PetId) ?? throw new KeyNotFoundException($"Pet with ID {request.PetId} not found");
            return mapper.Map<PetDto>(pet);
        }
    }
}
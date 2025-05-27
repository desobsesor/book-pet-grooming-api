using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Pets.Queries
{
    /// <summary>
    /// Handler to process the GetPetsByCustomerIdQuery and return a customer's list of pets
    /// </summary>
    public class GetPetsByCustomerIdQueryHandler(IPetRepository petRepository, IMapper mapper) : IRequestHandler<GetPetsByCustomerIdQuery, List<PetDto>>
    {
        public async Task<List<PetDto>> Handle(GetPetsByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            var pets = await petRepository.GetByCustomerIdAsync(request.CustomerId);
            return mapper.Map<List<PetDto>>(pets);
        }
    }
}
using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Pets.Queries
{
    /// <summary>
    /// Handler to process the GetPetsByCustomerIdQuery and return a customer's list of pets
    /// </summary>
    public class GetPetsByCustomerIdQueryHandler : IRequestHandler<GetPetsByCustomerIdQuery, List<PetDto>>
    {
        private readonly IPetRepository _petRepository;
        private readonly IMapper _mapper;

        public GetPetsByCustomerIdQueryHandler(IPetRepository petRepository, IMapper mapper)
        {
            _petRepository = petRepository;
            _mapper = mapper;
        }

        public async Task<List<PetDto>> Handle(GetPetsByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            var pets = await _petRepository.GetByCustomerIdAsync(request.CustomerId);
            return _mapper.Map<List<PetDto>>(pets);
        }
    }
}
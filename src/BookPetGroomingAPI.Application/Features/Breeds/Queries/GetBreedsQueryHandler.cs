using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Breeds.Queries
{
    /// <summary>
    /// Handler to process the GetBreedsQuery query and return the list of breeds
    /// </summary>
    public class GetBreedsQueryHandler(IBreedRepository breedRepository, IMapper mapper)
        : IRequestHandler<GetBreedsQuery, List<BreedDto>>
    {
        public async Task<List<BreedDto>> Handle(GetBreedsQuery request, CancellationToken cancellationToken)
        {
            var breeds = await breedRepository.GetAllAsync();
            return mapper.Map<List<BreedDto>>(breeds);
        }
    }
}
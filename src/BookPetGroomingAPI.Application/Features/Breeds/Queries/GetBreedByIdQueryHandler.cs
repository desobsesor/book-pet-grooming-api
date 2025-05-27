using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.Breeds.Queries
{
    /// <summary>
    /// Handler para procesar la consulta GetBreedByIdQuery y devolver la raza por ID
    /// </summary>
    public class GetBreedByIdQueryHandler(IBreedRepository breedRepository, IMapper mapper)
        : IRequestHandler<GetBreedByIdQuery, BreedDto>
    {
        public async Task<BreedDto> Handle(GetBreedByIdQuery request, CancellationToken cancellationToken)
        {
            var breed = await breedRepository.GetByIdAsync(request.BreedId);
            return mapper.Map<BreedDto>(breed);
        }
    }
}
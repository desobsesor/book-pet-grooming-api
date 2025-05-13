using AutoMapper;
using BookPetGroomingAPI.Domain.Interfaces;
using MediatR;

namespace BookPetGroomingAPI.Application.Features.PetCategories.Queries
{
    /// <summary>
    /// Handler for processing the GetPetCategoriesQuery and returning the list of petCategories
    /// </summary>
    public class GetPetCategoriesQueryHandler : IRequestHandler<GetPetCategoriesQuery, List<PetCategoryDto>>
    {
        private readonly IPetCategoryRepository _petCategoryRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for GetPetCategoriesQueryHandler
        /// </summary>
        /// <param name="petCategoryRepository">Repository for petCategory operations</param>
        /// <param name="mapper">Mapping service</param>
        public GetPetCategoriesQueryHandler(IPetCategoryRepository petCategoryRepository, IMapper mapper)
        {
            _petCategoryRepository = petCategoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the query to retrieve all petCategories
        /// </summary>
        /// <param name="request">The GetPetCategoriesQuery</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of petCategory DTOs</returns>
        public async Task<List<PetCategoryDto>> Handle(GetPetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var petCategories = await _petCategoryRepository.GetAllAsync();
            return _mapper.Map<List<PetCategoryDto>>(petCategories);
        }
    }
}
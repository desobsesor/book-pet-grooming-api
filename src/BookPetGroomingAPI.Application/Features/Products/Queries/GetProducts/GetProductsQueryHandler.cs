using AutoMapper;
using MediatR;
using BookPetGroomingAPI.Domain.Interfaces;

namespace BookPetGroomingAPI.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        //Get all products from the repository
        var products = await _productRepository.GetAllAsync();

        // Map products to DTOs using AutoMapper
        var productsDto = _mapper.Map<List<ProductDto>>(products);

        return productsDto;
    }
}
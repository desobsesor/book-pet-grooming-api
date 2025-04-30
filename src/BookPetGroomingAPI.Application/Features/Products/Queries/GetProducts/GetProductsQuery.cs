using MediatR;

namespace BookPetGroomingAPI.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery : IRequest<List<ProductDto>>
{
    // This query does not require parameters as it returns all products
}
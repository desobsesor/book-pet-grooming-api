using MediatR;
using BookPetGroomingAPI.Application.Features.Products.Queries.GetProducts;

namespace BookPetGroomingAPI.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(int Id) : IRequest<ProductDto>;
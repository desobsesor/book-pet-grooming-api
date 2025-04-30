using MediatR;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.Domain.Interfaces;

namespace BookPetGroomingAPI.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Handles the creation of a new product.
/// </summary>
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handles the CreateProductCommand and persists a new product in the database.
    /// </summary>
    /// <param name="request">The command containing product data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the newly created product.</returns>
    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Create a new instance of Product using the domain constructor
        var product = new Product(
            name: request.Name,
            description: request.Description,
            price: request.Price,
            stock: request.Stock);

        // Persist the product in the database
        var productId = await _productRepository.AddAsync(product);

        return productId;
    }
}
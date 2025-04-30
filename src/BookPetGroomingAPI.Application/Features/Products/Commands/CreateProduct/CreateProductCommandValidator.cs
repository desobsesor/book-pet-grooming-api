using FluentValidation;
using BookPetGroomingAPI.Domain.Interfaces;

namespace BookPetGroomingAPI.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Validator for CreateProductCommand. Ensures product data integrity and business rules.
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("The name is required")
            .MaximumLength(100).WithMessage("The name cannot exceed 100 characters")
            .MustAsync(async (name, cancellationToken) =>
                !(await _productRepository.ProductExistsAsync(name)))
            .WithMessage("A product with this name already exists");

        RuleFor(p => p.Description)
            .MaximumLength(500).WithMessage("The description cannot exceed 500 characters");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("The price must be greater than zero");

        RuleFor(p => p.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative");
    }
}
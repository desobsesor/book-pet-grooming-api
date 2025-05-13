using BookPetGroomingAPI.API.Controllers;
using BookPetGroomingAPI.Application.Features.Products.Commands;
using BookPetGroomingAPI.Application.Features.Products.Commands.CreateProduct;
using BookPetGroomingAPI.Application.Features.Products.Queries;
using BookPetGroomingAPI.Application.Features.Products.Queries.GetProductById;
using BookPetGroomingAPI.Application.Features.Products.Queries.GetProducts;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BookPetGroomingAPI.UnitTests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductsController _controller;
        private readonly ILogger<ProductsController> _logger;

        public ProductsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _logger = new Mock<ILogger<ProductsController>>().Object;
            _controller = new ProductsController(_mediatorMock.Object, _logger);
        }

        [Fact]
        public async Task GetProductById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var productId = 10;
            var query = new GetProductByIdQuery(productId);
            var product = new ProductDto { Id = productId, Name = "Test Product" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            result.Should().BeOfType<ActionResult<ProductDto>>();
            var okResult = (result.Result as OkObjectResult);
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new() { Id = 1, Name = "Producto 1", Price = 100 },
                new() { Id = 2, Name = "Producto 2", Price = 200 }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            result.Should().BeOfType<ActionResult<List<ProductDto>>>();
            var okResult = (result.Result as OkObjectResult);
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task CreateProduct_WithValidCommand_ReturnsCreatedAtAction()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "New Product",
                Description = "Description of product",
                Price = 150,
                Stock = 10
            };
            var productId = 1;
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productId);

            // Act
            var result = await _controller.CreateProduct(command);

            // Assert
            result.Should().BeOfType<ActionResult<int>>();
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult!.ActionName.Should().Be(nameof(ProductsController.GetProductById));
            createdAtActionResult.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(productId);
            createdAtActionResult.Value.Should().Be(productId);
        }
    }
}
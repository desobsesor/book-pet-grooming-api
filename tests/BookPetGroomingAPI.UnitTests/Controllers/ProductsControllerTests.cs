using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using BookPetGroomingAPI.Application.Features.Products.Commands.CreateProduct;
using BookPetGroomingAPI.Application.Features.Products.Queries.GetProducts;
using BookPetGroomingAPI.Application.Features.Products.Queries.GetProductById;
using BookPetGroomingAPI.Domain.Entities;
using BookPetGroomingAPI.API.Controllers;
using MediatR;

namespace BookPetGroomingAPI.UnitTests.API.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProductsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetProductById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var productId = 10;
            var query = new GetProductByIdQuery(productId);
            var product = new ProductoDto { Id = productId, Nombre = "Test Product" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            result.Should().BeOfType<ActionResult<ProductoDto>>();
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
                new() { Id = 1, Nombre = "Producto 1", Precio = 100 },
                new() { Id = 2, Nombre = "Producto 2", Precio = 200 }
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
using BookPetGroomingAPI.API.Controllers;
using BookPetGroomingAPI.Application.Features.Customers.Commands;
using BookPetGroomingAPI.Application.Features.Customers.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BookPetGroomingAPI.UnitTests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CustomerController _controller;
        private readonly ILogger<CustomerController> _logger;

        public CustomerControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _logger = new Mock<ILogger<CustomerController>>().Object;
            _controller = new CustomerController(_mediatorMock.Object, _logger);
        }

        [Fact]
        public async Task GetCustomerById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var customerId = 10;
            var query = new GetCustomerByIdQuery(customerId);
            var customer = new CustomerDto
            {
                CustomerId = customerId,
                FirstName = "Test",
                LastName = "Customer",
                Email = "test.customer@example.com",
                Phone = "555-123-4567",
                Address = "123 Test Street",
                PreferredGroomerId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerById(customerId);

            // Assert
            result.Should().BeOfType<ActionResult<CustomerDto>>();
            var okResult = (result.Result as OkObjectResult);
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(customer);
        }

        [Fact]
        public async Task GetCustomers_ReturnsOkResult()
        {
            // Arrange
            var customers = new List<CustomerDto>
            {
                new() { CustomerId = 1, FirstName = "Test", LastName = "Customer 1", Email = "test1@example.com", Phone = "555-111-1111", Address = "111 Test Avenue" },
                new() { CustomerId = 2, FirstName = "Test", LastName = "Customer 2", Email = "test2@example.com", Phone = "555-222-2222", Address = "222 Test Boulevard" }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(customers);

            // Act
            var result = await _controller.GetCustomers();

            // Assert
            result.Should().BeOfType<ActionResult<List<CustomerDto>>>();
            var okResult = (result.Result as OkObjectResult);
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(customers);
        }

        [Fact]
        public async Task CreateCustomer_WithValidCommand_ReturnsCreatedAtAction()
        {
            // Arrange
            var command = new CreateCustomerCommand(
                firstName: "New",
                lastName: "Test Customer",
                email: "new.test@example.com",
                phone: "555-333-4444",
                address: "333 Test Drive"
            );

            var customerId = 1;
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(customerId);

            // Act
            var result = await _controller.CreateCustomer(command);

            // Assert
            result.Should().BeOfType<ActionResult<int>>();
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult!.ActionName.Should().Be(nameof(CustomerController.GetCustomerById));
            createdAtActionResult.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(customerId);
            createdAtActionResult.Value.Should().Be(customerId);
        }

        [Fact]
        public async Task GetCustomerById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var customerId = 999;
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CustomerDto)null);

            // Act
            var result = await _controller.GetCustomerById(customerId);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateCustomer_WhenExceptionOccurs_ReturnsBadRequest()
        {
            // Arrange
            var command = new CreateCustomerCommand(
                firstName: "Exception",
                lastName: "Test Customer",
                email: "exception.test@example.com",
                phone: "555-999-9999",
                address: "999 Exception Avenue"
            );
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.CreateCustomer(command);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.Value.Should().Be("Error creating customer: Test exception");
        }
    }
}
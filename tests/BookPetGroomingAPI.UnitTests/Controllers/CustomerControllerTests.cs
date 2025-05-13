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
                FirstName = "Yovany",
                LastName = "Suarez Silva",
                Email = "yovanysuarezsilva@gmail.com",
                Phone = "PHONE",
                Address = "ADDRESS",
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
                new() { CustomerId = 1, FirstName = "Test Customer 1", LastName = "Test Customer 1", Email = "EMAIL1", Phone = "PHONE", Address = "ADDRESS" },
                new() { CustomerId = 2, FirstName = "Test Customer 2", LastName = "Test Customer 2", Email = "EMAIL2", Phone = "PHONE", Address = "ADDRESS" }
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
                firstName: "Yovany",
                lastName: "Suarez Silva",
                email: "yovanysuarezsilva@gmail.com",
                phone: "PHONE",
                address: "ADDRESS"
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
    }
}
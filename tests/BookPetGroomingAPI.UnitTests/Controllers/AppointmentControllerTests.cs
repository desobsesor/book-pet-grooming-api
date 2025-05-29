using BookPetGroomingAPI.API.Controllers;
using BookPetGroomingAPI.Application.Features.Appointments.Commands;
using BookPetGroomingAPI.Application.Features.Appointments.Queries;
using BookPetGroomingAPI.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BookPetGroomingAPI.UnitTests.Controllers
{
    public class AppointmentControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<AppointmentController>> _loggerMock;
        private readonly AppointmentController _controller;

        public AppointmentControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<AppointmentController>>();
            _controller = new AppointmentController(_mediatorMock.Object, _loggerMock.Object)
            {
                // Required to mock HttpContext in tests of methods that use it
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public async Task GetAppointments_ReturnsOkResult()
        {
            // Arrange
            var paginatedList = new PaginatedList<AppointmentDto>([], 0, 1, 10);
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentsQuery>(), default))
                .ReturnsAsync(paginatedList);

            // Act
            var result = await _controller.GetAppointments();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(paginatedList, okResult.Value);
        }

        [Fact]
        public async Task GetAppointments_HandlesException_Returns500()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentsQuery>(), default))
                .ThrowsAsync(new Exception("Test error"));

            // Act
            var result = await _controller.GetAppointments();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateAppointment_ReturnsCreatedAtAction()
        {
            // Arrange
            var command = new CreateAppointmentCommand(1, 1, DateTime.Now, TimeOnly.FromDateTime(DateTime.Now), 30, "pending", 100, "notes", 1);
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(42);

            // Act
            var result = await _controller.CreateAppointment(command);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(42, createdResult.Value);
        }

        [Fact]
        public async Task CreateAppointment_HandlesException_Returns500()
        {
            // Arrange
            var command = new CreateAppointmentCommand(1, 1, DateTime.Now, TimeOnly.FromDateTime(DateTime.Now), 30, "pending", 100, "notes", 1);
            _mediatorMock.Setup(m => m.Send(command, default)).ThrowsAsync(new Exception("Test error"));

            // Act
            var result = await _controller.CreateAppointment(command);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetAppointmentById_ReturnsOkResult()
        {
            // Arrange
            var dto = new AppointmentDto();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentByIdQuery>(), default)).ReturnsAsync(dto);

            // Act
            var result = await _controller.GetAppointmentById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public async Task GetAppointmentById_HandlesException_Returns500()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentByIdQuery>(), default)).ThrowsAsync(new Exception("Test error"));

            // Act
            var result = await _controller.GetAppointmentById(1);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetAppointmentsByGroomerId_ReturnsOkResult()
        {
            // Arrange
            var paginatedList = new PaginatedList<AppointmentDto>(new List<AppointmentDto>(), 0, 1, 10);
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentsByGroomerIdQuery>(), default)).ReturnsAsync(paginatedList);

            // Act
            var result = await _controller.GetAppointmentsByGroomerId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(paginatedList, okResult.Value);
        }

        [Fact]
        public async Task GetAppointmentsByPetId_ReturnsOkResult()
        {
            // Arrange
            var list = new List<AppointmentDto>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentsByPetIdQuery>(), default)).ReturnsAsync(list);

            // Act
            var result = await _controller.GetAppointmentsByPetId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(list, okResult.Value);
        }

        [Fact]
        public async Task GetAppointmentsByAppointmentDate_ReturnsOkResult()
        {
            // Arrange
            var list = new List<AppointmentDto>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentsByAppointmentDateQuery>(), default)).ReturnsAsync(list);

            // Act
            var result = await _controller.GetAppointmentsByAppointmentDate(DateTime.Today);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(list, okResult.Value);
        }

        [Fact]
        public async Task GetAppointmentsByStatus_ReturnsOkResult()
        {
            // Arrange
            var list = new List<AppointmentDto>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentsByStatusQuery>(), default)).ReturnsAsync(list);

            // Act
            var result = await _controller.GetAppointmentsByStatus("pending");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(list, okResult.Value);
        }

        [Fact]
        public async Task DeleteAppointment_ReturnsNoContent()
        {
            // Arrange
            var appointmentId = 1;
            _mediatorMock.Setup(m => m.Send(It.Is<DeleteAppointmentCommand>(cmd => cmd.AppointmentId == appointmentId), default));

            // Act
            var result = await _controller.DeleteAppointment(appointmentId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteAppointmentCommand>(), default), Times.Once);
        }
    }
}
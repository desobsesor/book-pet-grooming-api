using BookPetGroomingAPI.Application.Features.Customers.Commands;
using BookPetGroomingAPI.Application.Features.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookPetGroomingAPI.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/customers")]
public class CustomerController : ApiControllerBase
{
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(IMediator mediator, ILogger<CustomerController> logger) : base(mediator)
    {
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all customers
    /// </summary>
    /// <returns>List of customers</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CustomerDto>>> GetCustomers()
    {
        try
        {
            // Remove Console.WriteLine as we're using proper logging
            _logger.LogInformation("Getting list of all customers");

            var query = new GetCustomersQuery();
            var customers = await Mediator(query);

            _logger.LogInformation("Successfully retrieved {Count} customers", customers.Count);
            return Ok(customers);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving customer list. RequestId: {RequestId}. Details: {Message}",
                requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Creates a new customer
    /// </summary>
    /// <param name="command">Customer data to create</param>
    /// <returns>Created customer ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateCustomer([FromBody] CreateCustomerCommand command)
    {
        try
        {
            _logger.LogInformation("Starting customer creation: {FirstName} {LastName}", command.FirstName, command.LastName);

            var customerId = await Mediator(command);

            _logger.LogInformation("Customer successfully created with ID: {CustomerId}", customerId);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customerId }, customerId);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error creating customer {FirstName} {LastName}. RequestId: {RequestId}. Details: {Message}",
                command.FirstName, command.LastName, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Retrieves a customer by its ID
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Customer data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
    {
        try
        {
            _logger.LogInformation("Looking for customer with ID: {CustomerId}", id);

            var query = new GetCustomerByIdQuery(id);
            var customer = await Mediator(query);

            _logger.LogInformation("Customer with ID: {CustomerId} successfully retrieved", id);
            return Ok(customer);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving customer with ID: {CustomerId}. RequestId: {RequestId}. Details: {Message}",
                id, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }
}
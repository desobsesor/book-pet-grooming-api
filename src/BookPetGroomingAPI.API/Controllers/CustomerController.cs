using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.Customers.Commands;
using BookPetGroomingAPI.Application.Features.Customers.Queries;

namespace BookPetGroomingAPI.API.Controllers;

public class CustomerController : ApiControllerBase
{
    public CustomerController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Retrieves all customers
    /// </summary>
    /// <returns>List of customers</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CustomerDto>>> GetCustomers()
    {
        var query = new GetCustomersQuery();
        var customers = await Mediator(query);
        return Ok(customers);
    }

    /// <summary>
    /// Creates a new customer
    /// </summary>
    /// <param name="command">Customer data to create</param>
    /// <returns>Created customer ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateCustomer([FromBody] CreateCustomerCommand command)
    {
        var customerId = await Mediator(command);
        return CreatedAtAction(nameof(GetCustomerById), new { id = customerId }, customerId);
    }

    /// <summary>
    /// Retrieves a customer by its ID
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Customer data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
    {
        var query = new GetCustomerByIdQuery(id);
        var customer = await Mediator(query);
        return Ok(customer);
    }
}
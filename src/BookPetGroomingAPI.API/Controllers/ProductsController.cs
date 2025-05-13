using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.Products.Commands.CreateProduct;
using BookPetGroomingAPI.Application.Features.Products.Queries.GetProducts;
using BookPetGroomingAPI.Application.Features.Products.Queries.GetProductById;

namespace BookPetGroomingAPI.API.Controllers;

[Route("api/products")]
public class ProductsController : ApiControllerBase
{
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IMediator mediator, ILogger<ProductsController> logger) : base(mediator)
    {
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all products
    /// </summary>
    /// <returns>List of products</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        try
        {
            _logger.LogInformation("Getting list of all products");

            var query = new GetProductsQuery();
            var products = await Mediator(query);

            _logger.LogInformation("Successfully retrieved {Count} products", products.Count);
            return Ok(products);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving product list. RequestId: {RequestId}. Details: {Message}",
                requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="command">Product data to create</param>
    /// <returns>Created product ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateProduct([FromBody] CreateProductCommand command)
    {
        try
        {
            _logger.LogInformation("Starting product creation: {Name}", command.Name);

            var productId = await Mediator(command);

            _logger.LogInformation("Product successfully created with ID: {ProductId}", productId);
            return CreatedAtAction(nameof(GetProductById), new { id = productId }, productId);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error creating product {Name}. RequestId: {RequestId}. Details: {Message}",
                command.Name, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }

    /// <summary>
    /// Retrieves a product by its ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        try
        {
            _logger.LogInformation("Looking for product with ID: {ProductId}", id);

            var query = new GetProductByIdQuery(id);
            var product = await Mediator(query);

            _logger.LogInformation("Product with ID: {ProductId} successfully retrieved", id);
            return Ok(product);
        }
        catch (Exception ex)
        {
            var requestId = HttpContext.TraceIdentifier;
            _logger.LogError(ex, "Error retrieving product with ID: {ProductId}. RequestId: {RequestId}. Details: {Message}",
                id, requestId, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Server error processing request", requestId, message = "Check logs for more details" });
        }
    }
}
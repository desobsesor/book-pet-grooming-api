using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookPetGroomingAPI.Application.Features.Products.Commands.CreateProduct;
using BookPetGroomingAPI.Application.Features.Products.Queries.GetProducts;
using BookPetGroomingAPI.Application.Features.Products.Queries.GetProductById;

namespace BookPetGroomingAPI.API.Controllers;

public class ProductsController(IMediator mediator) : ApiControllerBase(mediator)
{

    /// <summary>
    /// Retrieves all products
    /// </summary>
    /// <returns>List of products</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        var query = new GetProductsQuery();
        var products = await Mediator(query);
        return Ok(products);
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="command">Product data to create</param>
    /// <returns>Created product ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateProduct([FromBody] CreateProductCommand command)
    {
        var productId = await Mediator(command);
        return CreatedAtAction(nameof(GetProductById), new { id = productId }, productId);
    }

    /// <summary>
    /// Retrieves a product by its ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product data</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var query = new GetProductByIdQuery(id);
        var product = await Mediator(query);
        return Ok(product);
    }
}
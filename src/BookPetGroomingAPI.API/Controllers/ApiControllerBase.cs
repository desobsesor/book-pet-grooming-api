using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookPetGroomingAPI.API.Controllers;

/// <summary>
/// Base controller for all API controllers
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Base controller constructor
    /// </summary>
    /// <param name="mediator">Instance of MediatR for the mediator pattern</param>
    protected ApiControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Sends a request through the mediator
    /// </summary>
    /// <typeparam name="TResponse">Expected response type</typeparam>
    /// <param name="request">Request to send</param>
    /// <returns>Response from the mediator</returns>
    protected async Task<TResponse> Mediator<TResponse>(IRequest<TResponse> request)
    {
        return await _mediator.Send(request);
    }
}
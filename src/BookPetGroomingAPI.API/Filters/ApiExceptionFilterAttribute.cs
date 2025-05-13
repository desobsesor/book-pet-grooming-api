using BookPetGroomingAPI.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookPetGroomingAPI.API.Filters;

/// <summary>
/// Filter to handle specific API exceptions and convert them into appropriate HTTP responses
/// </summary>
public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    /// <summary>
    /// Exception filter constructor
    /// </summary>
    public ApiExceptionFilterAttribute()
    {
        // Simplified constructor: we no longer need to initialize a dictionary
        // because we use pattern matching directly in the HandleException method
    }

    /// <summary>
    /// Method executed when an exception occurs
    /// </summary>
    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        // Optimization: Using pattern matching to handle exceptions more directly
        // and avoid dictionary lookups when possible
        switch (context.Exception)
        {
            case ValidationException validationException:
                HandleValidationException(context);
                return;

            case NotFoundException notFoundException:
                HandleNotFoundException(context);
                return;

            case UnauthorizedAccessException unauthorizedAccessException:
                HandleUnauthorizedAccessException(context);
                return;

            default:
                // If it's not a specific type handled by pattern matching,
                // we continue with ModelState verification

                // Check if ModelState is invalid
                if (!context.ModelState.IsValid)
                {
                    HandleInvalidModelStateException(context);
                }
                return;
        }
    }

    private static void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;
        var details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;
        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };

        context.ExceptionHandled = true;
    }

    private static void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }
}
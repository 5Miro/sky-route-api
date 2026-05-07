using System.Text.Json;
using SkyRoute.Api.Application.Exceptions;

namespace SkyRoute.Api.Middleware;

public sealed class ApiExceptionMiddleware
{
    private readonly ILogger<ApiExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (RequestValidationException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";

            var payload = new
            {
                type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                title = "One or more validation errors occurred.",
                status = StatusCodes.Status400BadRequest,
                errors = exception.Errors
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception while processing request.");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            var payload = new
            {
                title = "An unexpected error occurred.",
                status = StatusCodes.Status500InternalServerError
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}

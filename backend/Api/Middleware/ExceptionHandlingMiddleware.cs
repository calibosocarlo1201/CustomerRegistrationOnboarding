using System.Net;
using System.Text.Json;
using CustomerRegistrationOnboarding.Application.Exceptions;

namespace CustomerRegistrationOnboarding.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        object response;

        switch (exception)
        {
            case AppValidationException validationEx:
                statusCode = HttpStatusCode.BadRequest;
                response = new { title = "Validation failed", errors = validationEx.Errors };
                break;
            case NotFoundException notFoundEx:
                statusCode = HttpStatusCode.NotFound;
                response = new { title = notFoundEx.Message };
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                response = new { title = "An unexpected error occurred." };
                _logger.LogError(exception, "Unhandled exception occurred.");
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

using System.Net;
using System.Text.Json;
using Pokemon.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Pokemon.Web.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ExceptionHandlingMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case NotFoundException notFoundEx:
                code = HttpStatusCode.NotFound;
                result = CreateErrorResponse(notFoundEx.Message);
                break;

            case ValidationException validationEx:
                code = HttpStatusCode.BadRequest;
                result = CreateErrorResponse(validationEx.Message, validationEx.Errors);
                break;

            case UnauthorizedException:
                code = HttpStatusCode.Unauthorized;
                result = CreateErrorResponse(exception.Message);
                break;

            case ForbiddenException:
                code = HttpStatusCode.Forbidden;
                result = CreateErrorResponse(exception.Message);
                break;

            case DbUpdateException dbEx:
                code = HttpStatusCode.InternalServerError;
                _logger.LogError(dbEx, "Database error: {Message}", dbEx.Message);
                result = CreateErrorResponse(
                    "An error occurred while accessing the database",
                    details: _environment.IsDevelopment() ? dbEx.InnerException?.Message : null
                );
                break;

            case TimeoutException:
                code = HttpStatusCode.GatewayTimeout;
                result = CreateErrorResponse("The operation has exceeded the timeout period");
                break;

            default:
                code = HttpStatusCode.InternalServerError;
                result = CreateErrorResponse(
                    "An internal server error has occurred",
                    details: _environment.IsDevelopment() ? exception.ToString() : null
                );
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }

    private string CreateErrorResponse(string message, IEnumerable<string>? errors = null, string? details = null)
    {
        var response = new
        {
            error = message,
            errors = errors,
            details = details,
            timestamp = DateTime.UtcNow
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _environment.IsDevelopment()
        };

        return JsonSerializer.Serialize(response, options);
    }
}
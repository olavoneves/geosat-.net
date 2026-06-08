using GeoSat.API.DTOs.Response;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace GeoSat.API.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, error) = exception switch
        {
            KeyNotFoundException => (404, "Not Found"),
            UnauthorizedAccessException => (401, "Unauthorized"),
            ArgumentException => (400, "Bad Request"),
            InvalidOperationException => (422, "Unprocessable Entity"),
            DbUpdateException => (409, "Conflict"),
            _ => (500, "Internal Server Error")
        };

        if (status >= 500)
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        httpContext.Response.StatusCode = status;
        httpContext.Response.ContentType = "application/json";

        var response = new ErrorResponse(
            Status: status,
            Error: error,
            Message: exception.Message,
            Path: httpContext.Request.Path,
            Timestamp: DateTime.UtcNow
        );

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}

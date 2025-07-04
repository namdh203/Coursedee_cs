using Coursedee.Api.Common;
using System.Net;
using System.Text.Json;
using Coursedee.Api.Exceptions;

namespace Coursedee.Api.Middleware;

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
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            UnauthorizedException => (HttpStatusCode.Unauthorized, exception.Message), // 401
            ForbiddenException => (HttpStatusCode.Forbidden, exception.Message), // 403
            NotFoundException => (HttpStatusCode.NotFound, exception.Message), // 404
            BadRequestException => (HttpStatusCode.BadRequest, exception.Message), // 400
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred") // 500
        };

        response.StatusCode = (int)statusCode;

        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var errorResponse = ApiResponse<object>.ResponseGeneral(false, message, new { });

        // change object error response to JSON, maybe
        var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(result);
    }
} 
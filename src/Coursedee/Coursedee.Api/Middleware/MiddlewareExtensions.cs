namespace Coursedee.Api.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static IApplicationBuilder UseUserContext(this IApplicationBuilder app)
    {
        return app.UseMiddleware<UseUserContextMiddleware>();
    }
} 
using Coursedee.Application.Services;
using Coursedee.Application.Models;
using System.Security.Claims;

namespace Coursedee.Api.Middleware;

public class UseUserContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UseUserContextMiddleware> _logger;

    public UseUserContextMiddleware(RequestDelegate next, ILogger<UseUserContextMiddleware> logger) 
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IUserContextAccessor userContextAccessor)
    {
        if (context.User.Identity is not ClaimsIdentity identity)
        {
            _logger.LogDebug("No ClaimsIdentity found, skipping user context setup");
            await _next(context);
            return;
        }

        if (!identity.IsAuthenticated)
        {
            _logger.LogDebug("User not authenticated, skipping user context setup");
            await _next(context);
            return;
        }

        var userId = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogDebug("No user ID found in claims, skipping user context setup");
            await _next(context);
            return;
        }
        
        if (!long.TryParse(userId, out var id))
        {
            _logger.LogWarning("Invalid user ID format: {UserId}", userId);
            await _next(context);
            return;
        }

        var accessToken = context.Request.Headers["Authorization"]
            .ToString()
            .Replace("bearer ", string.Empty)
            .Replace("Bearer ", string.Empty);

        if (string.IsNullOrEmpty(accessToken))
        {
            _logger.LogDebug("No access token found, skipping user context setup");
            await _next(context);
            return;
        }

        var userContext = new UserContext
        {
            AccessToken = accessToken,
            UserId = id
        };

        userContextAccessor.Set(userContext);

        await _next(context);
    }
}
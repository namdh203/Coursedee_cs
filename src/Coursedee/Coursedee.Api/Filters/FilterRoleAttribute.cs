using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Coursedee.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursedee.Api.Filters;

public class FilterRole : AuthorizeAttribute, IAuthorizationFilter
{
    public new UserRole[] Roles { get; }

    public FilterRole(params UserRole[] roles)
    {
        Roles = roles;
        AuthenticationSchemes = "Bearer";
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user.Identity?.IsAuthenticated != true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userRoleClaim = user.FindFirst(ClaimTypes.Role)?.Value;
        if (string.IsNullOrEmpty(userRoleClaim))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!Enum.TryParse<UserRole>(userRoleClaim, out var userRole))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!Roles.Contains(userRole))
        {
            context.Result = new ForbidResult();
            return;
        }
    }

}
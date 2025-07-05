using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Coursedee.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Coursedee.Api.Common;
using System.Text.Json;

namespace Coursedee.Api.Filters;

public class FilterRole: AuthorizeAttribute, IAuthorizationFilter
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
            var response = ApiResponse<object>.ResponseGeneral(false, "Authentication required", null);
            context.Result = new JsonResult(response)
            {
                StatusCode = 401
            };
            return;
        }

        var userRoleClaim = user.FindFirst(ClaimTypes.Role)?.Value;
        if (string.IsNullOrEmpty(userRoleClaim))
        {
            var response = ApiResponse<object>.ResponseGeneral(false, "User role not found", null);
            context.Result = new JsonResult(response)
            {
                StatusCode = 401
            };
            return;
        }

        if (!Enum.TryParse<UserRole>(userRoleClaim, out var userRole))
        {
            var response = ApiResponse<object>.ResponseGeneral(false, "Invalid user role", null);
            context.Result = new JsonResult(response)
            {
                StatusCode = 401
            };
            return;
        }

        if (!Roles.Contains(userRole))
        {
            var response = ApiResponse<object>.ResponseGeneral(false, "You don't have permission to access this resource", null);
            context.Result = new JsonResult(response)
            {
                StatusCode = 401
            };
            return;
        }
    }

}
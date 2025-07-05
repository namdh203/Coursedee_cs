using Coursedee.Application.Data.Entities;
using Coursedee.Application.Models;
using System.Security.Claims;
using UserEntity = Coursedee.Application.Data.Entities.User;

namespace Coursedee.Application.Services;

public interface IJwtService
{
    string GenerateToken(UserEntity user);
    ClaimsPrincipal? ValidateToken(string token);
    string GetUserRoleFromToken(string token);
} 
using Coursedee.Application.Data.Entities;
using Coursedee.Application.Data.Repositories;
using Coursedee.Application.Models;
using Coursedee.Application.Services;
using System.Security.Cryptography;
using System.Text;
using UserEntity = Coursedee.Application.Data.Entities.User;

namespace Coursedee.Application.UseCase;

public interface IAuthUseCase
{
    Task<(UserEntity? user, string token)> LoginAsync(string email, string password);
    Task<(UserEntity? user, string token)> RegisterAsync(string name, string email, string password, UserRole role);
}

public class AuthUseCase : IAuthUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthUseCase(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<(UserEntity? user, string token)> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            return (null, string.Empty);

        var hashedPassword = HashPassword(password);
        if (user.PasswordDigest != hashedPassword)
            return (null, string.Empty);

        var token = _jwtService.GenerateToken(user);
        return (user, token);
    }

    public async Task<(UserEntity? user, string token)> RegisterAsync(string name, string email, string password, UserRole role)
    {
        var existingUser = await _userRepository.GetByEmailAsync(email);
        if (existingUser != null)
            return (null, string.Empty);

        var hashedPassword = HashPassword(password);
        var newUser = new UserEntity
        {
            Name = name,
            Email = email,
            PasswordDigest = hashedPassword,
            Role = (int)role
        };

        var createdUser = await _userRepository.CreateAsync(newUser);
        var token = _jwtService.GenerateToken(createdUser);
        
        return (createdUser, token);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
} 
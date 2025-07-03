using System.ComponentModel.DataAnnotations;
using Coursedee.Application.Models;

namespace Coursedee.Api.DTOs;

public record LoginRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
    
    [Required]
    public string Password { get; init; } = string.Empty;
}

public record RegisterRequestDto
{
    [Required]
    [StringLength(255)]
    public string Name { get; init; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
    
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; init; } = string.Empty;
    
    [Required]
    public UserRole Role { get; init; } = UserRole.Student;
} 
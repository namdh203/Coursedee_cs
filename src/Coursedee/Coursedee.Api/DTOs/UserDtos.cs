using Coursedee.Application.Models;

namespace Coursedee.Api.DTOs;

public record UserResponseDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public int Role { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}

public record UsersListResponseDto
{
    public List<UserResponseDto> Users { get; init; } = new();
}
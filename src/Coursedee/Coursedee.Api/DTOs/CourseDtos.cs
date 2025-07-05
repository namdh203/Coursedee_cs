using Coursedee.Application.Models;

namespace Coursedee.Api.DTOs;

public record CourseResponseDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public long TeacherId { get; init; }
}

public record CourseRequestDto
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
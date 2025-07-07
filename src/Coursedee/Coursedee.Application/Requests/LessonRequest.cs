using System.ComponentModel.DataAnnotations;

namespace Coursedee.Application.Requests;

public class LessonRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
}
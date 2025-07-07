using System.ComponentModel.DataAnnotations;

namespace Coursedee.Application.Requests;

public class CourseRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
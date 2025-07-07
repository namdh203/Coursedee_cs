using System.ComponentModel.DataAnnotations;

namespace Coursedee.Application.Requests;

public class ReviewRequest
{
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }
    [Required]
    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;
}
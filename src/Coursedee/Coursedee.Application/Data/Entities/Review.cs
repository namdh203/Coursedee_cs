using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Coursedee.Application.Data.Entities.Common;

namespace Coursedee.Application.Data.Entities;

public class Review : BaseEntity
{
    [Key]
    [Column(TypeName = "bigint")]
    public long Id { get; set; }

    [Required]
    [Column(TypeName = "bigint")]
    public long StudentId { get; set; }

    [Required]
    [Column(TypeName = "bigint")]
    public long CourseId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;

    [ForeignKey("StudentId")]
    public User Student { get; set; } = null!;

    [ForeignKey("CourseId")]
    public Course Course { get; set; } = null!;
}
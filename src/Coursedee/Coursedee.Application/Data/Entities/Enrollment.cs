using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Coursedee.Application.Data.Entities.Common;

namespace Coursedee.Application.Data.Entities;

public class Enrollment : BaseEntity
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

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTimeOffset EnrolledAt { get; set; }

    [ForeignKey("StudentId")]
    public User Student { get; set; } = null!;

    [ForeignKey("CourseId")]
    public Course Course { get; set; } = null!;
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Coursedee.Application.Data.Entities.Common;

namespace Coursedee.Application.Data.Entities;

public class LessonProcess : BaseEntity
{
    [Key]
    [Column(TypeName = "bigint")]
    public long Id { get; set; }

    [Required]
    [Column(TypeName = "bigint")]
    public long StudentId { get; set; }

    [Required]
    [Column(TypeName = "bigint")]
    public long LessonId { get; set; }

    public bool Done { get; set; }

    public DateTimeOffset? CompeleteAt { get; set; }

    [ForeignKey("StudentId")]
    public User Student { get; set; } = null!;

    [ForeignKey("LessonId")]
    public Lesson Lesson { get; set; } = null!;
}
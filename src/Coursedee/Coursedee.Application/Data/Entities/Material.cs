using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Coursedee.Application.Data.Entities.Common;

namespace Coursedee.Application.Data.Entities;

public class Material : BaseEntity
{
    [Key]
    [Column(TypeName = "bigint")]
    public long Id { get; set; }

    [Required]
    [Column(TypeName = "bigint")]
    public long LessonId { get; set; }

    [Required]
    [MaxLength(500)]
    public string FileUrl { get; set; } = string.Empty;

    [Required]
    public int FileType { get; set; }

    [ForeignKey("LessonId")]
    public Lesson Lesson { get; set; } = null!;
}
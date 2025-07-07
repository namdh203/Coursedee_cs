using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Coursedee.Application.Data.Entities.Common;

namespace Coursedee.Application.Data.Entities;

public class Course : BaseEntity
{
    [Key]
    [Column(TypeName = "bigint")]
    public long Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "bigint")]
    public long TeacherId { get; set; }

    [ForeignKey("TeacherId")]
    public virtual User Teacher { get; set; } = null!;
    
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
} 
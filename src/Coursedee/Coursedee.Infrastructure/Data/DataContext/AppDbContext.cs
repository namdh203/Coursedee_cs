using Coursedee.Application.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coursedee.Infrastructure.Data.DataContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnType("bigint");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordDigest).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Role).IsRequired();
            entity.Property(e => e.ResetPasswordToken).HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnType("bigint");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.TeacherId).IsRequired().HasColumnType("bigint");
            entity.HasOne(e => e.Teacher)
                .WithMany(e => e.TeachingCourses)
                .HasForeignKey(e => e.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnType("bigint");
            entity.Property(e => e.CourseId).IsRequired().HasColumnType("bigint");
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Content).IsRequired();
            entity.HasOne(e => e.Course)
                .WithMany(e => e.Lessons)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

    }
}
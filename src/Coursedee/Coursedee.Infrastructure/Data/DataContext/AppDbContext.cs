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
    public DbSet<Material> Materials { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<LessonProcess> LessonProcesses { get; set; }
    public DbSet<Review> Reviews { get; set; }

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

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnType("bigint");
            entity.Property(e => e.LessonId).IsRequired().HasColumnType("bigint");
            entity.Property(e => e.FileUrl).IsRequired().HasMaxLength(500);
            entity.Property(e => e.FileType).IsRequired();
            entity.HasOne(e => e.Lesson)
                .WithMany(e => e.Materials)
                .HasForeignKey(e => e.LessonId);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnType("bigint");
            entity.Property(e => e.StudentId).IsRequired().HasColumnType("bigint");
            entity.Property(e => e.CourseId).IsRequired().HasColumnType("bigint");

            entity.HasOne(e => e.Student)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Course)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<LessonProcess>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnType("bigint");
            entity.Property(e => e.StudentId).IsRequired().HasColumnType("bigint");
            entity.Property(e => e.LessonId).IsRequired().HasColumnType("bigint");
            entity.Property(e => e.Done).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();

            entity.HasOne(e => e.Student)
                .WithMany(e => e.LessonProcesses)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Lesson)
                .WithMany(e => e.LessonProcesses)
                .HasForeignKey(e => e.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnType("bigint");
            entity.Property(e => e.StudentId).IsRequired().HasColumnType("bigint");
            entity.Property(e => e.CourseId).IsRequired().HasColumnType("bigint");
            entity.Property(e => e.Rating).IsRequired();
            entity.Property(e => e.Comment).IsRequired();

            entity.HasOne(e => e.Student)
                .WithMany(e => e.Reviews)
                .HasForeignKey(e => e.StudentId);

            entity.HasOne(e => e.Course)
                .WithMany(e => e.Reviews)
                .HasForeignKey(e => e.CourseId);
        });

    }
}
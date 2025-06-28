using Coursedee.Application.Data.Entities;
using Coursedee.Application.Data.Repositories;
using Coursedee.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Coursedee.Infrastructure.Data.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Course?> GetByIdAsync(long id)
    {
        return await _context.Courses
            .Include(c => c.Teacher)
            .Include(c => c.Lessons)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses
            .Include(c => c.Teacher)
            .Include(c => c.Lessons)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetByTeacherIdAsync(long teacherId)
    {
        return await _context.Courses
            .Include(c => c.Teacher)
            .Include(c => c.Lessons)
            .Where(c => c.TeacherId == teacherId)
            .ToListAsync();
    }

    public async Task<Course> CreateAsync(Course course)
    {
        course.CreatedAt = DateTime.UtcNow;
        course.UpdatedAt = DateTime.UtcNow;
        
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task<Course> UpdateAsync(Course course)
    {
        course.UpdatedAt = DateTime.UtcNow;
        
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task DeleteAsync(long id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course != null)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(long id)
    {
        return await _context.Courses.AnyAsync(c => c.Id == id);
    }
}
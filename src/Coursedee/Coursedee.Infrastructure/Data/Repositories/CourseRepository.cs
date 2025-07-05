using Coursedee.Application.Data.Repositories;
using Coursedee.Application.Data.Entities;
using Coursedee.Infrastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Coursedee.Infrastructure.Data.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Course>> GetAllAsync()
    {
        return await _context.Courses.ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(long id)
    {
        return await _context.Courses.FindAsync(id);
    }

    public async Task AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            throw new Exception("Course not found");
        }
        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
    }
}
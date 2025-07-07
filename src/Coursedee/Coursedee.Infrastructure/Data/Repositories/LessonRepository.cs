using Coursedee.Application.Data.Repositories;
using Coursedee.Infrastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;
using Coursedee.Application.Data.Entities;
using System.Runtime.InteropServices;

namespace Coursedee.Infrastructure.Data.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly AppDbContext _context;

    public LessonRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Lesson>> GetAllAsync()
    {
        return await _context.Lessons.ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(long id)
    {
        return await _context.Lessons.FindAsync(id);
    }

    public async Task AddAsync(Lesson lesson)
    {
        await _context.Lessons.AddAsync(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var lesson = await _context.Lessons.FindAsync(id);
        if (lesson == null)
        {
            throw new Exception("Lesson not found");
        }
        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();
    }
}
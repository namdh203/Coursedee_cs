using Coursedee.Application.Data.Repositories;
using Coursedee.Infrastructure.Data.DataContext;
using Coursedee.Application.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coursedee.Infrastructure.Data.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetAllAsync()
    {
        return await _context.Reviews.ToListAsync();
    }

    public async Task<Review?> GetByIdAsync(long id)
    {
        return await _context.Reviews.FindAsync(id);
    }

    public async Task AddAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Review review)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            throw new Exception("Review not found");
        }
        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }

    public async Task<Review?> GetByStudentAndCourseAsync(long studentId, long courseId)
    {
        return await _context.Reviews
            .FirstOrDefaultAsync(r => r.StudentId == studentId && r.CourseId == courseId);
    }
}
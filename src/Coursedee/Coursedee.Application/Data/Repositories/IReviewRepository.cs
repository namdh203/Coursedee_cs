using Coursedee.Application.Data.Entities;

namespace Coursedee.Application.Data.Repositories;

public interface IReviewRepository
{
    Task<List<Review>> GetAllAsync();
    Task<Review?> GetByIdAsync(long id);
    Task AddAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(long id);
    Task<Review?> GetByStudentAndCourseAsync(long studentId, long courseId);
}
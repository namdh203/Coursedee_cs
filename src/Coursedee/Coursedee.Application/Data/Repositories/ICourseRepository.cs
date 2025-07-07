using Coursedee.Application.Data.Entities;

namespace Coursedee.Application.Data.Repositories;

public interface ICourseRepository
{
    Task<List<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(long id);
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(long id);
}
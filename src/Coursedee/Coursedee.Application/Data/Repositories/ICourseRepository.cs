using Coursedee.Application.Data.Entities;

namespace Coursedee.Application.Data.Repositories;

public interface ICourseRepository
{
  Task<List<Course>> GetAllAsync();
  Task<Course> GetByIdAsync(long id);
  Task<Course> AddAsync(Course course);
  Task<Course> UpdateAsync(Course course);
  Task<Course> DeleteAsync(long id);
}
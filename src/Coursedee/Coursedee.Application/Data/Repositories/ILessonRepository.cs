using Coursedee.Application.Data.Entities;

namespace Coursedee.Application.Data.Repositories;

public interface ILessonRepository
{
    Task<List<Lesson>> GetAllAsync();
    Task<Lesson?> GetByIdAsync(long id);
    Task AddAsync(Lesson lesson);
    Task UpdateAsync(Lesson lesson);
    Task DeleteAsync(long id);
}
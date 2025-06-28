using Coursedee.Application.Data.Repositories;

namespace Coursedee.Infrastructure.Data.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly AppDbContext _context;

    public LessonRepository(AppDbContext context)
    {
        _context = context;
    }
}
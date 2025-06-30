using Coursedee.Application.Data.Repositories;
using Coursedee.Infrastructure.Data.DataContext;

namespace Coursedee.Infrastructure.Data.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly AppDbContext _context;

    public LessonRepository(AppDbContext context)
    {
        _context = context;
    }
}
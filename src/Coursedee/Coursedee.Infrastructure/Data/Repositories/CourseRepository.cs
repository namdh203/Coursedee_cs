using Coursedee.Application.Data.Repositories;

namespace Coursedee.Infrastructure.Data.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }
}
using Coursedee.Application.Data.Entities;
using Coursedee.Application.Data.Repositories;
using Coursedee.Infrastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Coursedee.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
}
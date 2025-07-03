using Coursedee.Application.Data.Entities;

namespace Coursedee.Application.Data.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
}
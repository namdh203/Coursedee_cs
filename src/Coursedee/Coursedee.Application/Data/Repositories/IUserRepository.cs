using Coursedee.Application.Data.Entities;

namespace Coursedee.Application.Data.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
}
using Coursedee.Application.Data.Entities;

namespace Coursedee.Application.Data.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
}
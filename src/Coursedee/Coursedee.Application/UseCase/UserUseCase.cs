using AutoMapper;
using Coursedee.Application.Data.Repositories;
using Coursedee.Application.Models;

namespace Coursedee.Application.UseCase;

public interface IUserUseCase
{
  Task<IEnumerable<User>> GetAllUsersAsync();
}

public class UserUseCase : IUserUseCase
{
  private readonly IMapper _mapper;
  private readonly IUserRepository _userRepository;

  public UserUseCase(IUserRepository userRepository, IMapper mapper)
  {
    _userRepository = userRepository;
    _mapper = mapper;
  }

  public async Task<IEnumerable<User>> GetAllUsersAsync()
  {
    return _mapper.Map<IEnumerable<User>>(await _userRepository.GetAllAsync());
  }
}
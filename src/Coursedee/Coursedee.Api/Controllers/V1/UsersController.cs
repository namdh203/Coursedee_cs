using Coursedee.Application.Data.Entities;
using Coursedee.Application.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Coursedee.Api.Controllers.V1;

[ApiController]
[Route("api/V1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, "Internal server error");
        }
    }
} 
using Coursedee.Application.UseCase;
using Coursedee.Application.Models;
using Coursedee.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Coursedee.Api.Controllers.V1;

[ApiController]
[Route("api/V1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserUseCase _userUseCase;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserUseCase userUseCase, ILogger<UsersController> logger)
    {
        _userUseCase = userUseCase;
        _logger = logger;
    }

    [HttpGet]
    [FilterRole(UserRole.Admin)]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        try
        {
            return Ok(new
            {
                success = true,
                message = "Users retrieved successfully",
                data = await _userUseCase.GetAllUsersAsync()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, new { success = false, message = "Internal server error", data = new { } });
        }
    }
} 
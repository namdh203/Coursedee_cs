using Coursedee.Application.UseCase;
using Coursedee.Application.Models;
using Coursedee.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Coursedee.Api.Common;

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<User>>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<object>))]
    public async Task<ActionResult<ApiResponse<List<User>>>> GetUsers()
    {
        var users = await _userUseCase.GetAllUsersAsync();
        return Ok(ApiResponse<List<User>>.ResponseGeneral(true, "Users retrieved successfully", users));
    }
} 
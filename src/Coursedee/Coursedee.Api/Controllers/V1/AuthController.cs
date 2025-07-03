using Coursedee.Application.UseCase;
using Coursedee.Application.Models;
using Coursedee.Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Coursedee.Api.Controllers.V1;

[ApiController]
[Route("api/V1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthUseCase _authUseCase;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthUseCase authUseCase, ILogger<AuthController> logger)
    {
        _authUseCase = authUseCase;
        _logger = logger;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var (user, token) = await _authUseCase.LoginAsync(request.Email, request.Password);
            
            if (user == null)
                return BadRequest(new { success = false, message = "Invalid email or password", data = new { } });

            return Ok(new
            {
                success = true,
                message = "Login successful",
                data = new {
                  user = new {
                      user.Id,
                      user.Name,
                      user.Email,
                      Role = (UserRole)user.Role,
                      token
                  }
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(500, new { success = false, message = "Internal server error", data = new { } });
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<object>> Register([FromBody] RegisterRequestDto request)
    {
        try
        {
            var (user, token) = await _authUseCase.RegisterAsync(request.Name, request.Email, request.Password, request.Role);
            
            if (user == null)
                return BadRequest(new { success = false, message = "User with this email already exists", data = new { } });

            return Ok(new
            {
                success = true,
                message = "Registration successful",
                data = new
                {
                    user = new {
                      user.Id,
                      user.Name,
                      user.Email,
                      Role = (UserRole)user.Role,
                      token
                  }
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return StatusCode(500, new { success = false, message = "Internal server error", data = new {} });
        }
    }
}
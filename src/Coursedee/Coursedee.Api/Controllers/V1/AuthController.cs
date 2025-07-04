using Coursedee.Application.UseCase;
using Coursedee.Api.DTOs;
using Coursedee.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Coursedee.Api.Common;

namespace Coursedee.Api.Controllers.V1;

[ApiController]
[Route("api/V1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthUseCase _authUseCase;

    public AuthController(IAuthUseCase authUseCase)
    {
        _authUseCase = authUseCase;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> Login([FromBody] LoginRequestDto request)
    {
        var (user, token) = await _authUseCase.LoginAsync(request.Email, request.Password);
        
        if (user == null)
            throw new BadRequestException("Invalid email or password");

        var authResponse = new AuthResponseDto
        {
            User = user,
            Token = token
        };

        return Ok(ApiResponse<object>.ResponseGeneral(true, "Login successful", authResponse));
    }

    [HttpPost("register")]
    public async Task<ActionResult<object>> Register([FromBody] RegisterRequestDto request)
    {
        var (user, token) = await _authUseCase.RegisterAsync(request.Name, request.Email, request.Password, request.Role);
        
        if (user == null)
            throw new BadRequestException("User with this email already exists");

        var authResponse = new AuthResponseDto
        {
            User = user,
            Token = token
        };

        return Ok(ApiResponse<object>.ResponseGeneral(true, "Registration successful", authResponse));
    }
} 
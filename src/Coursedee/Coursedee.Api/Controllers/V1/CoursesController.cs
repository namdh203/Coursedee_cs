using Coursedee.Application.UseCase;
using Coursedee.Application.Models;
using Coursedee.Api.Filters;
using Coursedee.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Coursedee.Api.Common;
using Coursedee.Application.Data.Entities;
using System.Security.Claims;
using CourseEntity = Coursedee.Application.Data.Entities.Course; 
using Coursedee.Application.Services;

namespace Coursedee.Api.Controllers.V1;

[ApiController]
[Route("api/V1/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseUseCase _courseUseCase;
    private readonly ILogger<CoursesController> _logger;
    private readonly IUserContextAccessor _userContextAccessor;

    public CoursesController(
        ICourseUseCase courseUseCase, 
        ILogger<CoursesController> logger, 
        IUserContextAccessor userContextAccessor)
    {
        _courseUseCase = courseUseCase;
        _logger = logger;
        _userContextAccessor = userContextAccessor;
    }

    [HttpGet]
    [FilterRole(UserRole.Admin, UserRole.Teacher, UserRole.Student)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<CourseResponseDto>>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<List<CourseResponseDto>>>> GetCourses()
    {
        var courses = await _courseUseCase.GetAllCoursesAsync();
        var courseDtos = courses.Select(course => new CourseResponseDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            TeacherId = course.TeacherId
        }).ToList();
        return Ok(ApiResponse<List<CourseResponseDto>>.ResponseGeneral(true, "Courses fetched successfully", courseDtos));
    }

    [HttpGet("{id}")]
    [FilterRole(UserRole.Admin, UserRole.Teacher, UserRole.Student)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<CourseResponseDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<CourseResponseDto>>> GetCoursesById(long id)
    {
        var course = await _courseUseCase.GetCourseByIdAsync(id);
        if (course == null)
        {
            return NotFound(ApiResponse<CourseResponseDto>.ResponseGeneral(false, "Course not found", null));
        }
        var courseDto = new CourseResponseDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            TeacherId = course.TeacherId
        };
        return Ok(ApiResponse<CourseResponseDto>.ResponseGeneral(true, "Course fetched successfully", courseDto));
    }

    [HttpPost]
    [FilterRole(UserRole.Teacher)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<CourseResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<CourseResponseDto>>> CreateCourse([FromBody] CourseRequestDto request)
    {
        var currentUser = _userContextAccessor.Get();
        _logger.LogInformation("Current user: {CurrentUser}", currentUser);
        if (currentUser == null)
        {
            return Unauthorized(ApiResponse<CourseResponseDto>.ResponseGeneral(false, "User not authenticated", null));
        }

        var course = new CourseEntity
        {
            Name = request.Name,
            Description = request.Description,
            TeacherId = currentUser.UserId
        };

        await _courseUseCase.CreateCourseAsync(course);

        return CreatedAtAction(nameof(GetCourses), new { id = course.Id }, 
            ApiResponse<CourseResponseDto>.ResponseGeneral(true, "Course created successfully", null));
    }

    [HttpPut("{id}")]
    [FilterRole(UserRole.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<CourseResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<CourseResponseDto>>> UpdateCourse(long id, [FromBody] CourseRequestDto request)
    {
        var currentUser = _userContextAccessor.Get();
        if (currentUser == null)
        {
            return Unauthorized(ApiResponse<CourseResponseDto>.ResponseGeneral(false, "User not authenticated", null));
        }

        var existingCourse = await _courseUseCase.GetCourseByIdAsync(id);
        if (existingCourse == null)
        {
            return NotFound(ApiResponse<CourseResponseDto>.ResponseGeneral(false, "Course not found", null));
        }

        if (existingCourse.TeacherId != currentUser.UserId)
        {
            return Unauthorized(ApiResponse<CourseResponseDto>.ResponseGeneral(false, "You are not the owner of this course", null));
        }

        existingCourse.Name = request.Name;
        existingCourse.Description = request.Description;
        existingCourse.UpdatedAt = DateTimeOffset.UtcNow;

        await _courseUseCase.UpdateCourseAsync(existingCourse);

        return Ok(ApiResponse<CourseResponseDto>.ResponseGeneral(true, "Course updated successfully", null));
    }

    [HttpDelete("{id}")]
    [FilterRole(UserRole.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<CourseResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<CourseResponseDto>>> DeleteCourse(long id)
    {
        var currentUser = _userContextAccessor.Get();
        if (currentUser == null)
        {
            return Unauthorized(ApiResponse<CourseResponseDto>.ResponseGeneral(false, "User not authenticated", null));
        }

        var existingCourse = await _courseUseCase.GetCourseByIdAsync(id);

        if (existingCourse == null)
        {
            return NotFound(ApiResponse<CourseResponseDto>.ResponseGeneral(false, "Course not found", null));
        }

        if (existingCourse.TeacherId != currentUser.UserId)
        {
            return Unauthorized(ApiResponse<CourseResponseDto>.ResponseGeneral(false, "You are not the owner of this course", null));
        }
        await _courseUseCase.DeleteCourseAsync(id);

        return Ok(ApiResponse<CourseResponseDto>.ResponseGeneral(true, "Course deleted successfully", null));
    }
}
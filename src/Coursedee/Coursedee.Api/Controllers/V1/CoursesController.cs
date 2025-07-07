using Coursedee.Application.UseCase;
using Coursedee.Application.Models;
using Coursedee.Application.Requests;
using Coursedee.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Coursedee.Api.Common;
using Coursedee.Application.Data.Entities;
using System.Security.Claims;

using CourseEntity = Coursedee.Application.Data.Entities.Course; 
using CourseModel = Coursedee.Application.Models.Course;
using Coursedee.Application.Services;
using AutoMapper;

namespace Coursedee.Api.Controllers.V1;

[ApiController]
[Route("api/V1/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseUseCase _courseUseCase;
    private readonly ILogger<CoursesController> _logger;
    private readonly IUserContextAccessor _userContextAccessor;
    private readonly IMapper _mapper;
    public CoursesController(
        ICourseUseCase courseUseCase, 
        ILogger<CoursesController> logger, 
        IUserContextAccessor userContextAccessor,
        IMapper mapper)
    {
        _courseUseCase = courseUseCase;
        _logger = logger;
        _userContextAccessor = userContextAccessor;
        _mapper = mapper;
    }

    [HttpGet]
    [FilterRole(UserRole.Admin, UserRole.Teacher, UserRole.Student)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<CourseModel>>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<List<CourseModel>>>> GetCourses()
    {
        var courses = await _courseUseCase.GetAllCoursesAsync();
        return Ok(ApiResponse<List<CourseModel>>.ResponseGeneral(true, "Courses fetched successfully", courses));
    }

    [HttpGet("{id}")]
    [FilterRole(UserRole.Admin, UserRole.Teacher, UserRole.Student)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<CourseModel>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<CourseModel>>> GetCourseById(long id)
    {
        var course = await _courseUseCase.GetCourseByIdAsync(id);
        if (course == null)
        {
            return NotFound(ApiResponse<CourseModel>.ResponseGeneral(false, "Course not found", null));
        }
        return Ok(ApiResponse<CourseModel>.ResponseGeneral(true, "Course fetched successfully", _mapper.Map<CourseModel>(course)));
    }

    [HttpPost]
    [FilterRole(UserRole.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<CourseModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<CourseModel>>> CreateCourse([FromBody] CourseRequest request)
    {
        var currentUser = _userContextAccessor.Get();
        _logger.LogInformation("Current user: {CurrentUser}", currentUser);
        if (currentUser == null)
        {
            return Unauthorized(ApiResponse<CourseModel>.ResponseGeneral(false, "User not authenticated", null));
        }

        await _courseUseCase.CreateCourseAsync(new CourseEntity
        {
            Name = request.Name,
            Description = request.Description,
            TeacherId = currentUser.UserId
        });

        return Ok(ApiResponse<CourseModel>.ResponseGeneral(true, "Course created successfully", null));
    }

    [HttpPut("{id}")]
    [FilterRole(UserRole.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<CourseModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<CourseModel>>> UpdateCourse(long id, [FromBody] CourseRequest request)
    {
        var currentUser = _userContextAccessor.Get();
        if (currentUser == null)
        {
            return Unauthorized(ApiResponse<CourseModel>.ResponseGeneral(false, "User not authenticated", null));
        }

        var existingCourse = await _courseUseCase.GetCourseByIdAsync(id);
        if (existingCourse == null)
        {
            return NotFound(ApiResponse<CourseModel>.ResponseGeneral(false, "Course not found", null));
        }

        if (existingCourse.TeacherId != currentUser.UserId)
        {
            return Unauthorized(ApiResponse<CourseModel>.ResponseGeneral(false, "You are not the owner of this course", null));
        }

        existingCourse.Name = request.Name;
        existingCourse.Description = request.Description;
        existingCourse.UpdatedAt = DateTimeOffset.UtcNow;

        await _courseUseCase.UpdateCourseAsync(existingCourse);

        return Ok(ApiResponse<CourseModel>.ResponseGeneral(true, "Course updated successfully", null));
    }

    [HttpDelete("{id}")]
    [FilterRole(UserRole.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<CourseModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<CourseModel>>> DeleteCourse(long id)
    {
        var currentUser = _userContextAccessor.Get();
        if (currentUser == null)
        {
            return Unauthorized(ApiResponse<CourseModel>.ResponseGeneral(false, "User not authenticated", null));
        }

        var existingCourse = await _courseUseCase.GetCourseByIdAsync(id);

        if (existingCourse == null)
        {
            return NotFound(ApiResponse<CourseModel>.ResponseGeneral(false, "Course not found", null));
        }

        if (existingCourse.TeacherId != currentUser.UserId)
        {
            return Unauthorized(ApiResponse<CourseModel>.ResponseGeneral(false, "You are not the owner of this course", null));
        }
        await _courseUseCase.DeleteCourseAsync(id);

        return Ok(ApiResponse<CourseModel>.ResponseGeneral(true, "Course deleted successfully", null));
    }
}
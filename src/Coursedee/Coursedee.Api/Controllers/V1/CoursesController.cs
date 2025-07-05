using Coursedee.Application.UseCase;
using Coursedee.Application.Models;
using Coursedee.Api.Filters;
using Coursedee.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Coursedee.Api.Common;
using Coursedee.Application.Data.Entities;
using System.Security.Claims;

using CourseEntity = Coursedee.Application.Data.Entities.Course;

namespace Coursedee.Api.Controllers.V1;

[ApiController]
[Route("api/V1/[controller]")]
public class CoursesController : ControllerBase
{
  private readonly ICourseUseCase _courseUseCase;
  private readonly ILogger<CoursesController> _logger;

  public CoursesController(ICourseUseCase courseUseCase, ILogger<CoursesController> logger)
  {
    _courseUseCase = courseUseCase;
    _logger = logger;
  }

  [HttpGet]
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
      Description = course.Description
    }).ToList();
    return Ok(ApiResponse<List<CourseResponseDto>>.ResponseGeneral(true, "Courses fetched successfully", courseDtos));
  }

  [HttpPost]
  [FilterRole(UserRole.Teacher)]
  [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<CourseResponseDto>))]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<ApiResponse<CourseResponseDto>>> CreateCourse([FromBody] CourseRequestDto request)
  {
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    long.TryParse(userId, out long TeacherId);
    var course = new CourseEntity
    {
      Name = request.Name,
      Description = request.Description,
      TeacherId = TeacherId
    };

    var createdCourse = await _courseUseCase.CreateCourseAsync(course);
    var courseDto = new CourseResponseDto
    {
      Id = createdCourse.Id,
      Name = createdCourse.Name,
      Description = createdCourse.Description
    };

    return CreatedAtAction(nameof(GetCourses), new { id = createdCourse.Id },
      ApiResponse<CourseResponseDto>.ResponseGeneral(true, "Course created successfully", courseDto));
  }

  [HttpPut("{id}")]
  [FilterRole(UserRole.Teacher)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<CourseResponseDto>))]
  public async Task<ActionResult<ApiResponse<CourseResponseDto>>> UpdateCourse(long id, [FromBody] CourseRequestDto requestDto)
  {
    
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    long.TryParse(userId, out long TeacherId);

    var course = await _courseUseCase.GetCourseByIdAsync(id);

    var updatedCourse = await _courseUseCase.UpdateCourseAsync(course);
    var courseDto = new CourseResponseDto
    {
      Id = updatedCourse.Id,
      Name = updatedCourse.Name,
      Description = updatedCourse.Description
    };

    return Ok(ApiResponse<CourseResponseDto>.ResponseGeneral(true, "Course updated successfully", courseDto));
  }
}
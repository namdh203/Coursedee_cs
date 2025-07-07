using Coursedee.Application.UseCase;
using Coursedee.Application.Models;
using Coursedee.Application.Requests;
using Coursedee.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Coursedee.Api.Common;
using Coursedee.Application.Data.Entities;
using System.Security.Claims;

using LessonEntity = Coursedee.Application.Data.Entities.Lesson; 
using LessonModel = Coursedee.Application.Models.Lesson;
using Coursedee.Application.Services;
using AutoMapper;

namespace Coursedee.Api.Controllers.V1;

[ApiController]
[Route("api/V1/Course/{courseId}/[controller]")]
public class LessonController : ControllerBase
{
    private readonly ILessonUseCase _lessonUseCase;
    private readonly ICourseUseCase _courseUseCase;
    private readonly ILogger<LessonController> _logger;
    private readonly IUserContextAccessor _userContextAccessor;
    private readonly IMapper _mapper;
    public LessonController(
        ILessonUseCase lessonUseCase,
        ICourseUseCase courseUseCase,
        ILogger<LessonController> logger, 
        IUserContextAccessor userContextAccessor,
        IMapper mapper)
    {
        _lessonUseCase = lessonUseCase;
        _courseUseCase = courseUseCase;
        _logger = logger;
        _userContextAccessor = userContextAccessor;
        _mapper = mapper;
    }

    [HttpGet]
    [FilterRole(UserRole.Admin, UserRole.Teacher, UserRole.Student)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<LessonModel>>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<List<LessonModel>>>> GetLessons(long courseId)
    {
        var course = await _courseUseCase.GetCourseByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<List<LessonModel>>.ResponseGeneral(false, "Course not found", null));
        }

        var lessons = await _lessonUseCase.GetAllLessonsAsync();
        var courseLessons = lessons.Where(l => l.CourseId == courseId).ToList();
        return Ok(ApiResponse<List<LessonModel>>.ResponseGeneral(true, "Lessons fetched successfully", courseLessons));
    }

    [HttpGet("{id}")]
    [FilterRole(UserRole.Admin, UserRole.Teacher, UserRole.Student)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<LessonModel>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<LessonModel>>> GetLessonById(long courseId, long id)
    {
        var course = await _courseUseCase.GetCourseByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<LessonModel>.ResponseGeneral(false, "Course not found", null));
        }

        var lesson = await _lessonUseCase.GetLessonByIdAsync(id);
        if (lesson == null)
        {
            return NotFound(ApiResponse<LessonModel>.ResponseGeneral(false, "Lesson not found", null));
        }

        if (lesson.CourseId != courseId)
        {
            return NotFound(ApiResponse<LessonModel>.ResponseGeneral(false, "Lesson not found in this course", null));
        }

        return Ok(ApiResponse<LessonModel>.ResponseGeneral(true, "Lesson fetched successfully", _mapper.Map<LessonModel>(lesson)));
    }

    [HttpPost]
    [FilterRole(UserRole.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<LessonModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<LessonModel>>> CreateLesson(long courseId, [FromBody] LessonRequest request)
    {
        var currentUser = _userContextAccessor.Get();
        _logger.LogInformation("Current user: {CurrentUser}", currentUser);
        if (currentUser == null)
        {
            return Unauthorized(ApiResponse<LessonModel>.ResponseGeneral(false, "User not authenticated", null));
        }

        var course = await _courseUseCase.GetCourseByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<LessonModel>.ResponseGeneral(false, "Course not found", null));
        }

        if (course.TeacherId != currentUser.UserId)
        {
            return Unauthorized(ApiResponse<LessonModel>.ResponseGeneral(false, "You are not the owner of this course", null));
        }

        await _lessonUseCase.CreateLessonAsync(new LessonEntity
        {
            CourseId = courseId,
            Title = request.Title,
            Content = request.Content ?? string.Empty,
        });

        return Ok(ApiResponse<LessonModel>.ResponseGeneral(true, "Lesson created successfully", null));
    }

    [HttpPut("{id}")]
    [FilterRole(UserRole.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<LessonModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<LessonModel>>> UpdateLesson(long courseId, long id, [FromBody] LessonRequest request)
    {
        var currentUser = _userContextAccessor.Get();
        if (currentUser == null)
        {
            return Unauthorized(ApiResponse<LessonModel>.ResponseGeneral(false, "User not authenticated", null));
        }

        var course = await _courseUseCase.GetCourseByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<LessonModel>.ResponseGeneral(false, "Course not found", null));
        }

        if (course.TeacherId != currentUser.UserId)
        {
            return Unauthorized(ApiResponse<LessonModel>.ResponseGeneral(false, "You are not the owner of this course", null));
        }

        var existingLesson = await _lessonUseCase.GetLessonByIdAsync(id);
        if (existingLesson == null)
        {
            return NotFound(ApiResponse<LessonModel>.ResponseGeneral(false, "Lesson not found", null));
        }

        if (existingLesson.CourseId != courseId)
        {
            return NotFound(ApiResponse<LessonModel>.ResponseGeneral(false, "Lesson not found in this course", null));
        }

        existingLesson.Title = request.Title;
        existingLesson.Content = request.Content ?? string.Empty;
        existingLesson.UpdatedAt = DateTimeOffset.UtcNow;

        await _lessonUseCase.UpdateLessonAsync(existingLesson);

        return Ok(ApiResponse<LessonModel>.ResponseGeneral(true, "Lesson updated successfully", null));
    }

    [HttpDelete("{id}")]
    [FilterRole(UserRole.Teacher)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<LessonModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<LessonModel>>> DeleteLesson(long courseId, long id)
    {
        var currentUser = _userContextAccessor.Get();
        if (currentUser == null)
        {
            return Unauthorized(ApiResponse<LessonModel>.ResponseGeneral(false, "User not authenticated", null));
        }

        var course = await _courseUseCase.GetCourseByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<LessonModel>.ResponseGeneral(false, "Course not found", null));
        }

        if (course.TeacherId != currentUser.UserId)
        {
            return Unauthorized(ApiResponse<LessonModel>.ResponseGeneral(false, "You are not the owner of this course", null));
        }

        var existingLesson = await _lessonUseCase.GetLessonByIdAsync(id);

        if (existingLesson == null)
        {
            return NotFound(ApiResponse<LessonModel>.ResponseGeneral(false, "Lesson not found", null));
        }

        if (existingLesson.CourseId != courseId)
        {
            return NotFound(ApiResponse<LessonModel>.ResponseGeneral(false, "Lesson not found in this course", null));
        }

        await _lessonUseCase.DeleteLessonAsync(id);

        return Ok(ApiResponse<LessonModel>.ResponseGeneral(true, "Lesson deleted successfully", null));
    }
}

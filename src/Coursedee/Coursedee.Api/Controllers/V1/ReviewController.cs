using Coursedee.Application.UseCase;
using Coursedee.Application.Models;
using Coursedee.Application.Requests;
using Coursedee.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Coursedee.Api.Common;
using Coursedee.Application.Data.Entities;
using System.Security.Claims;

using ReviewEntity = Coursedee.Application.Data.Entities.Review; 
using ReviewModel = Coursedee.Application.Models.Review;
using Coursedee.Application.Services;
using AutoMapper;

namespace Coursedee.Api.Controllers.V1;

[ApiController]
[Route("api/V1/Course/{courseId}/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewUseCase _reviewUseCase;
    private readonly ICourseUseCase _courseUseCase;
    private readonly ILogger<ReviewController> _logger;
    private readonly IUserContextAccessor _userContextAccessor;
    private readonly IMapper _mapper;

    public ReviewController(
        IReviewUseCase reviewUseCase,
        ICourseUseCase courseUseCase,
        ILogger<ReviewController> logger, 
        IUserContextAccessor userContextAccessor,
        IMapper mapper)
    {
        _reviewUseCase = reviewUseCase;
        _courseUseCase = courseUseCase;
        _logger = logger;
        _userContextAccessor = userContextAccessor;
        _mapper = mapper;
    }

    [HttpGet]
    [FilterRole(UserRole.Admin, UserRole.Teacher, UserRole.Student)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<ReviewModel>>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<List<ReviewModel>>>> GetReviews(long courseId)
    {
        var course = await _courseUseCase.GetCourseByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<List<ReviewModel>>.ResponseGeneral(false, "Course not found", null));
        }

        var reviews = await _reviewUseCase.GetAllReviewsAsync();
        var courseReviews = reviews.Where(r => r.CourseId == courseId).ToList();
        return Ok(ApiResponse<List<ReviewModel>>.ResponseGeneral(true, "Reviews fetched successfully", courseReviews));
    }

    [HttpGet("{id}")]
    [FilterRole(UserRole.Admin, UserRole.Teacher, UserRole.Student)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ReviewModel>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ReviewModel>>> GetReviewById(long courseId, long id)
    {
        var course = await _courseUseCase.GetCourseByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<ReviewModel>.ResponseGeneral(false, "Course not found", null));
        }

        var review = await _reviewUseCase.GetReviewByIdAsync(id);
        if (review == null)
        {
            return NotFound(ApiResponse<ReviewModel>.ResponseGeneral(false, "Review not found", null));
        }

        if (review.CourseId != courseId)
        {
            return NotFound(ApiResponse<ReviewModel>.ResponseGeneral(false, "Review not found in this course", null));
        }

        return Ok(ApiResponse<ReviewModel>.ResponseGeneral(true, "Review fetched successfully", _mapper.Map<ReviewModel>(review)));
    }

    [HttpPost]
    [FilterRole(UserRole.Student)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ReviewModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ReviewModel>>> CreateReview(long courseId, [FromBody] ReviewRequest request)
    {
        var currentUser = _userContextAccessor.Get();
        _logger.LogInformation("Current user: {CurrentUser}", currentUser);
        if (currentUser == null)
        {
            return Unauthorized(ApiResponse<ReviewModel>.ResponseGeneral(false, "User not authenticated", null));
        }

        var course = await _courseUseCase.GetCourseByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<ReviewModel>.ResponseGeneral(false, "Course not found", null));
        }

        var existingReview = await _reviewUseCase.GetByStudentAndCourseAsync(currentUser.UserId, courseId);
        if (existingReview != null)
        {
            return BadRequest(ApiResponse<ReviewModel>.ResponseGeneral(false, "You have already reviewed this course.", null));
        }

        await _reviewUseCase.CreateReviewAsync(new ReviewEntity
        {
            CourseId = courseId,
            StudentId = currentUser.UserId,
            Rating = request.Rating,
            Comment = request.Comment,
        });

        return Ok(ApiResponse<ReviewModel>.ResponseGeneral(true, "Review created successfully", null));
    }

    [HttpPut("{id}")]
    [FilterRole(UserRole.Student)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ReviewModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<ReviewModel>>> UpdateReview(long courseId, long id, [FromBody] ReviewRequest request)
    {
        var currentUser = _userContextAccessor.Get();
        if (currentUser == null)
        {
            return Unauthorized(ApiResponse<ReviewModel>.ResponseGeneral(false, "User not authenticated", null));
        }

        var course = await _courseUseCase.GetCourseByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<ReviewModel>.ResponseGeneral(false, "Course not found", null));
        }

        var existingReview = await _reviewUseCase.GetReviewByIdAsync(id);
        if (existingReview == null)
        {
            return NotFound(ApiResponse<ReviewModel>.ResponseGeneral(false, "Review not found", null));
        }

        if (existingReview.CourseId != courseId)
        {
            return NotFound(ApiResponse<ReviewModel>.ResponseGeneral(false, "Review not found in this course", null));
        }

        existingReview.Rating = request.Rating;
        existingReview.Comment = request.Comment;
        existingReview.UpdatedAt = DateTimeOffset.UtcNow;

        await _reviewUseCase.UpdateReviewAsync(existingReview);

        return Ok(ApiResponse<ReviewModel>.ResponseGeneral(true, "Review updated successfully", null));
    }

    [HttpDelete("{id}")]
    [FilterRole(UserRole.Student, UserRole.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ReviewModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<ReviewModel>>> DeleteReview(long courseId, long id)
    {
        var currentUser = _userContextAccessor.Get();
        if (currentUser == null)
        {
            return Unauthorized(ApiResponse<ReviewModel>.ResponseGeneral(false, "User not authenticated", null));
        }

        var course = await _courseUseCase.GetCourseByIdAsync(courseId);
        if (course == null)
        {
            return NotFound(ApiResponse<ReviewModel>.ResponseGeneral(false, "Course not found", null));
        }

        var existingReview = await _reviewUseCase.GetReviewByIdAsync(id);

        if (existingReview == null)
        {
            return NotFound(ApiResponse<ReviewModel>.ResponseGeneral(false, "Review not found", null));
        }

        if (existingReview.CourseId != courseId)
        {
            return NotFound(ApiResponse<ReviewModel>.ResponseGeneral(false, "Review not found in this course", null));
        }

        await _reviewUseCase.DeleteReviewAsync(id);

        return Ok(ApiResponse<ReviewModel>.ResponseGeneral(true, "Review deleted successfully", null));
    }
}

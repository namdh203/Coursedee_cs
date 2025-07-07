using AutoMapper;
using Coursedee.Application.Models;
using Coursedee.Application.Data.Repositories;
using System.Transactions;

namespace Coursedee.Application.UseCase;

public interface IReviewUseCase
{
    Task<List<Review>> GetAllReviewsAsync();
    Task<Data.Entities.Review?> GetReviewByIdAsync(long id);
    Task CreateReviewAsync(Data.Entities.Review review);
    Task UpdateReviewAsync(Data.Entities.Review review);
    Task DeleteReviewAsync(long id);
    Task<Data.Entities.Review?> GetByStudentAndCourseAsync(long studentId, long courseId);
}

public class ReviewUseCase : IReviewUseCase
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public ReviewUseCase(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    public async Task<List<Review>> GetAllReviewsAsync()
    {
        return _mapper.Map<List<Review>>(await _reviewRepository.GetAllAsync());
    }

    public async Task<Data.Entities.Review?> GetReviewByIdAsync(long id)
    {
        return await _reviewRepository.GetByIdAsync(id);
    }

    public async Task CreateReviewAsync(Data.Entities.Review review)
    {
        // Ambient Transaction with 1 dbcontext
        using var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _reviewRepository.AddAsync(review);
        ts.Complete();
    }

    public async Task UpdateReviewAsync(Data.Entities.Review review)
    {
        using var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _reviewRepository.UpdateAsync(review);
        ts.Complete();
    }

    public async Task DeleteReviewAsync(long id)
    {
        using var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _reviewRepository.DeleteAsync(id);
        ts.Complete();
    }

    public async Task<Data.Entities.Review?> GetByStudentAndCourseAsync(long studentId, long courseId)
    {
        return await _reviewRepository.GetByStudentAndCourseAsync(studentId, courseId);
    }
}
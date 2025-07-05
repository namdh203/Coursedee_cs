using AutoMapper;
using Coursedee.Application.Models;
using Coursedee.Application.Data.Repositories;
using System.Transactions;

namespace Coursedee.Application.UseCase;

public interface ICourseUseCase
{
    Task<List<Course>> GetAllCoursesAsync();
    Task<Data.Entities.Course?> GetCourseByIdAsync(long id);
    Task CreateCourseAsync(Data.Entities.Course course);
    Task UpdateCourseAsync(Data.Entities.Course course);
    Task DeleteCourseAsync(long id);
}

public class CourseUseCase : ICourseUseCase
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public CourseUseCase(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        return _mapper.Map<List<Course>>(await _courseRepository.GetAllAsync());
    }

    public async Task<Data.Entities.Course?> GetCourseByIdAsync(long id)
    {
        return await _courseRepository.GetByIdAsync(id);
    }

    public async Task CreateCourseAsync(Data.Entities.Course course)
    {
        // Ambient Transaction with 1 dbcontext
        using var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _courseRepository.AddAsync(course);
        ts.Complete();
    }

    public async Task UpdateCourseAsync(Data.Entities.Course course)
    {
        using var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _courseRepository.UpdateAsync(course);
        ts.Complete();
    }

    public async Task DeleteCourseAsync(long id)
    {
        using var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _courseRepository.DeleteAsync(id);
        ts.Complete();
    }
}
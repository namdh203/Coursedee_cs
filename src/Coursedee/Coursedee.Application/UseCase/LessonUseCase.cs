using AutoMapper;
using Coursedee.Application.Models;
using Coursedee.Application.Data.Repositories;
using System.Transactions;

namespace Coursedee.Application.UseCase;

public interface ILessonUseCase
{
    Task<List<Lesson>> GetAllLessonsAsync();
    Task<Data.Entities.Lesson?> GetLessonByIdAsync(long id);
    Task CreateLessonAsync(Data.Entities.Lesson lesson);
    Task UpdateLessonAsync(Data.Entities.Lesson lesson);
    Task DeleteLessonAsync(long id);
}

public class LessonUseCase : ILessonUseCase
{
    private readonly ILessonRepository _lessonRepository;
    private readonly IMapper _mapper;

    public LessonUseCase(ILessonRepository lessonRepository, IMapper mapper)
    {
        _lessonRepository = lessonRepository;
        _mapper = mapper;
    }

    public async Task<List<Lesson>> GetAllLessonsAsync()
    {
        return _mapper.Map<List<Lesson>>(await _lessonRepository.GetAllAsync());
    }

    public async Task<Data.Entities.Lesson?> GetLessonByIdAsync(long id)
    {
        return await _lessonRepository.GetByIdAsync(id);
    }

    public async Task CreateLessonAsync(Data.Entities.Lesson lesson)
    {
        // Ambient Transaction with 1 dbcontext
        using var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _lessonRepository.AddAsync(lesson);
        ts.Complete();
    }

    public async Task UpdateLessonAsync(Data.Entities.Lesson lesson)
    {
        using var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _lessonRepository.UpdateAsync(lesson);
        ts.Complete();
    }

    public async Task DeleteLessonAsync(long id)
    {
        using var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await _lessonRepository.DeleteAsync(id);
        ts.Complete();
    }
}

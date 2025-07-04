using AutoMapper;
using Coursedee.Application.Models;
using Coursedee.Application.Data.Repositories;

namespace Coursedee.Application.UseCase;

public interface ICourseUseCase
{
  Task<List<Course>> GetAllCoursesAsync();
  Task<Course> GetCourseByIdAsync(int id);
  Task<Course> CreateCourseAsync(Data.Entities.Course course);
  Task<Course> UpdateCourseAsync(Data.Entities.Course course);
  Task<Course> DeleteCourseAsync(int id);
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

  public async Task<Course> GetCourseByIdAsync(int id)
  {
    return _mapper.Map<Course>(await _courseRepository.GetByIdAsync(id));
  }

  public async Task<Course> CreateCourseAsync(Data.Entities.Course course)
  {
    var newCourse = course;
    await _courseRepository.AddAsync(newCourse);
    return _mapper.Map<Course>(newCourse);
  }

  public async Task<Course> UpdateCourseAsync(Data.Entities.Course course)
  {
    var updatedCourse = course;
    await _courseRepository.UpdateAsync(updatedCourse);
    return _mapper.Map<Course>(updatedCourse);
  }

  public async Task<Course> DeleteCourseAsync(int id)
  {
    var deletedCourse = await _courseRepository.DeleteAsync(id);
    return _mapper.Map<Course>(deletedCourse);
  }
}
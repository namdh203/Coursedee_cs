using AutoMapper;

namespace Coursedee.Application.Models;

public class Lesson
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public long CourseId { get; set; }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Data.Entities.Lesson, Lesson>();
        }
    }
} 
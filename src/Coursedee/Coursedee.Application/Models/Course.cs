using AutoMapper;

namespace Coursedee.Application.Models;

public class Course
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long TeacherId { get; set;}

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Data.Entities.Course, Course>();
        }
    }
}
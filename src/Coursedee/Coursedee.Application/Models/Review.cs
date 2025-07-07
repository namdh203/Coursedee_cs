using AutoMapper;

namespace Coursedee.Application.Models;

public class Review
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Data.Entities.Review, Review>();
        }
    }
} 
using AutoMapper;
using Coursedee.Application.Data.Entities.Common;

namespace Coursedee.Application.Models;

public class User : BaseEntity
{
  public long Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string PasswordDigest { get; set; } = string.Empty;
  public int Role { get; set; }
  public string? ResetPasswordToken { get; set; }
  public DateTime? ResetPasswordSentAt { get; set; }

  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<Data.Entities.User, User>()
        .ForMember(dest => dest.PasswordDigest, opt => opt.Ignore());
    }
  }
}
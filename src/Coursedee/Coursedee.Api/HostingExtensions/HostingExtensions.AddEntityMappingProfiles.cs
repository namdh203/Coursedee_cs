using Coursedee.Application.Models;

namespace Coursedee.Api.HostingExtensions;

public partial class HostingExtensions
{
    public static void AddEntityMappingProfiles(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(User.MappingProfile));
    }
}
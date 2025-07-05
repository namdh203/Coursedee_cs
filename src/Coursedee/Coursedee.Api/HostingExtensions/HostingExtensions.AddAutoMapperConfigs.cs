using AutoMapper;

namespace Coursedee.Api.HostingExtensions;

public static partial class HostingExtensions
{
    public static IServiceCollection AddAutoMapperConfigs(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(GlobalMappingProfile));

        HostingExtensions.AddEntityMappingProfiles(services);
        
        return services;
    }
} 

public class GlobalMappingProfile : Profile
{
    public GlobalMappingProfile()
    {

    }
}
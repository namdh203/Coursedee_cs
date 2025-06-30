using Coursedee.Application.Data.Repositories;
using Coursedee.Infrastructure.Data.Repositories;

namespace Coursedee.Api.HostingExtensions;

public static partial class HostingExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ICourseRepository, CourseRepository>()
                .AddScoped<ILessonRepository, LessonRepository>();

        return services;
    }
}
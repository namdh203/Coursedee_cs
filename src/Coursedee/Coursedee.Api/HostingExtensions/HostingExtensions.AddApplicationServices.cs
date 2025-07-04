using Coursedee.Application.Data.Repositories;
using Coursedee.Infrastructure.Data.Repositories;
using Coursedee.Application.UseCase;
using Coursedee.Application.Services;

namespace Coursedee.Api.HostingExtensions;

public static partial class HostingExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ICourseRepository, CourseRepository>()
                .AddScoped<ILessonRepository, LessonRepository>();

        // UseCases
        services.AddScoped<IUserUseCase, UserUseCase>()
                .AddScoped<IAuthUseCase, AuthUseCase>()
                .AddScoped<ICourseUseCase, CourseUseCase>();

        // Services
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}
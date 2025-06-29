using Microsoft.EntityFrameworkCore;
using Coursedee.Infrastructure.Data;

namespace Coursedee.Api.HostingExtensions
{
    public static class DbServiceExtensions
    {
        public static IServiceCollection AddDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))
            ));

            return services;
        }
    }
}
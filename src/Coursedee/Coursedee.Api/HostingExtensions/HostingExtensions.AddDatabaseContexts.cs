using Coursedee.Infrastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Coursedee.Api.HostingExtensions;

public static partial class HostingExtensions
{
  public static IServiceCollection AddDatabaseContexts(this IServiceCollection services, IConfiguration configuration)
  {
      services.AddDbContext<AppDbContext>(options =>
      {
          var connectionString = configuration.GetConnectionString("DefaultConnection");
          options.UseMySql(
              connectionString,
              ServerVersion.AutoDetect(connectionString)
          );
      });

      return services;
  }
}
namespace Coursedee.Api.HostingExtensions
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");
            if (string.IsNullOrEmpty(redisConnectionString))
            {
                throw new ArgumentException("Redis connection string is not configured.");
            }

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "Coursedee_";
            });

            return services;
        }
    }
}

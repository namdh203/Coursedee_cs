namespace Coursedee.Api.HostingExtensions
{
    public static class LocalStackServiceExtensions
    {
        public static IServiceCollection AddLocalStackServices(this IServiceCollection services, IConfiguration configuration)
        {
            var localstackServiceUrl = configuration.GetConnectionString("Localstack");
            if (string.IsNullOrEmpty(localstackServiceUrl))
            {
                throw new ArgumentException("LocalStack service URL is not configured.");
            }

            services.AddHttpClient("LocalStackClient", client =>
            {
                client.BaseAddress = new Uri(localstackServiceUrl);
            });

            return services;
        }
    }
}
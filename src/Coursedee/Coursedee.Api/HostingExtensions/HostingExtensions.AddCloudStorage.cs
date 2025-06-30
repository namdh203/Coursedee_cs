using Amazon.S3;
using Amazon.Runtime;

namespace Coursedee.Api.HostingExtensions; 

public static partial class HostingExtensions
{
    public static IServiceCollection AddCloudStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var s3Config = new AmazonS3Config
        {
            ServiceURL = configuration["AWS:ServiceURL"],
            ForcePathStyle = true,
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(configuration["AWS:Region"])
        };

        var credentials = new BasicAWSCredentials("dummy", "dummy");
        var s3Client = new AmazonS3Client(credentials, s3Config);

        services.AddSingleton<IAmazonS3>(s3Client);

        return services;
    }
}
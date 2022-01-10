using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Ardalis.GuardClauses;
using Studens.Extensions.FileProviders;
using Studens.Extensions.FileProviders.Amazon;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for Amazon file manager registration
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAmazonFileManager(this IServiceCollection services, Action<AmazonFileManagerOptions> optionsAction)
    {
        Guard.Against.Null(services, nameof(services));
        Guard.Against.Null(optionsAction, nameof(optionsAction));

        services.Configure(optionsAction);
        // Validate credentials... and throw if not present
        var awsCredentials = new BasicAWSCredentials("", "");

        services.AddSingleton<IAmazonS3>(new AmazonS3Client(awsCredentials, RegionEndpoint.EUCentral1));
        services.AddScoped<IFileManager<AmazonPersistFileInfo>, AmazonFileManager>();
        services.AddScoped<FileProviderErrorDescriber>();

        return services;
    }
}
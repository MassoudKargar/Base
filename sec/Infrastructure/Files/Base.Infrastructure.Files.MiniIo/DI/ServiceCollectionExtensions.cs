using Base.Core.Domains.Contracts.Files;
using Base.Infrastructure.Files.MiniIo.Images;

using Microsoft.Extensions.Configuration;

namespace Base.Infrastructure.Files.MiniIo.DI;

public static class ServiceCollectionExtensions
{
    public static void AddMinIoFileUploaderServices(this IServiceCollection services, IConfiguration configuration)
    {
        var minIoMode = configuration.GetSection("Minio").GetValue<bool>("Enable");

        if (minIoMode)
        {
            services.Configure<MinIoOptions>(configuration.GetSection("Minio"));
            var config = configuration.GetSection("Minio").Get<MinIoOptions>();

            services.AddMinio(configureClient => configureClient
                .WithEndpoint(config.Endpoint)
                .WithCredentials(config.AccessKey, config.SecretKey)
                .WithSSL(false).Build());
            services.AddScoped<IFileUploaderService, MinIoUploaderService>();
            services.AddScoped<IImageCompressor, SkiaSharpImageCompressor>();
            services.AddScoped<IImageValidator, SkiaSharpImageValidator>();
            services.AddScoped<IImageLossLessCompressor, MagicNetImageCompressor>();
        }
        //else
        //{
        //    services.Configure<FtpOptions>(configuration.GetSection("Ftp"));
        //    services.AddScoped<IFileUploaderService, FtpFileUploaderService>();
        //}

        services.AddScoped<IValidateFileInformationService, ValidateFileInformationService>();
    }
}
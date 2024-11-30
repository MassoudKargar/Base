using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Base.Extensions.DependencyInjection;

public static class KafkaConsumerServiceCollectionExtensions
{
    public static IServiceCollection AddBaseKafkaConsumer(this IServiceCollection services, IConfiguration configuration)
    {        

        return services;
    }

}
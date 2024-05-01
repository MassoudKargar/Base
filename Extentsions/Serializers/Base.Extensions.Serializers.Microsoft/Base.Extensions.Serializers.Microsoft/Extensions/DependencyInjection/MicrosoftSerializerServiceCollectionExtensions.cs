using Microsoft.Extensions.DependencyInjection;
using Base.Extensions.Serializers.Abstractions;
using Base.Extensions.Serializers.Microsoft.Services;

namespace Base.Extensions.DependencyInjection;

public static class MicrosoftSerializerServiceCollectionExtensions
{
    public static IServiceCollection AddBaseMicrosoftSerializer(this IServiceCollection services)
        => services.AddSingleton<IJsonSerializer, MicrosoftSerializer>();
}

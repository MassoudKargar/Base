using Microsoft.Extensions.DependencyInjection;
using Base.Extensions.Serializers.NewtonSoft.Services;
using Base.Extensions.Serializers.Abstractions;

namespace Base.Extensions.DependencyInjection;

public static class NewtonSoftSerializerServiceCollectionExtensions
{
    public static IServiceCollection AddBaseNewtonSoftSerializer(this IServiceCollection services)
        => services.AddSingleton<IJsonSerializer, NewtonSoftSerializer>();
}
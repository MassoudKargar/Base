using Microsoft.Extensions.DependencyInjection;
using Base.Extensions.Serializers.EPPlus.Services;
using Base.Extensions.Serializers.Abstractions;

namespace Base.Extensions.DependencyInjection;

public static class EPPlusExcelSerializerServiceCollectionExtensions
{
    public static IServiceCollection AddEPPlusExcelSerializer(this IServiceCollection services)
        => services.AddSingleton<IExcelSerializer, EPPlusExcelSerializer>();
}
using Microsoft.Extensions.Logging;
using Zamin.Extensions.Caching.Abstractions;
using Zamin.Extensions.ObjectMappers.Abstractions;
using Zamin.Extensions.Serializers.Abstractions;
using Zamin.Extensions.Translations.Abstractions;
using Zamin.Extensions.UsersManagement.Abstractions;

namespace Base.Utilities;

public class BaseServices(
    ITranslator translator,
    ILoggerFactory loggerFactory,
    IJsonSerializer serializer,
    IUserInfoService userInfoService,
    ICacheAdapter cacheAdapter,
    IMapperAdapter mapperFacade)
{
    public readonly ITranslator Translator = translator;
    public readonly ICacheAdapter CacheAdapter = cacheAdapter;
    public readonly IMapperAdapter MapperFacade = mapperFacade;
    public readonly ILoggerFactory LoggerFactory = loggerFactory;
    public readonly IJsonSerializer Serializer = serializer;
    public readonly IUserInfoService UserInfoService = userInfoService;
}
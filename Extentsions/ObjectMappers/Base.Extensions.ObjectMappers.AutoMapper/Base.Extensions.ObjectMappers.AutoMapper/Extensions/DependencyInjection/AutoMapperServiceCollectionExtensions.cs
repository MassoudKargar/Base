namespace Base.Extensions.DependencyInjection;
public static class AutoMapperServiceCollectionExtensions
{
    public static IServiceCollection AddBaseAutoMapperProfiles(this IServiceCollection services,
                                                          IConfiguration configuration,
                                                          string sectionName)
        => services.AddBaseAutoMapperProfiles(configuration.GetSection(sectionName));

    public static IServiceCollection AddBaseAutoMapperProfiles(this IServiceCollection services, IConfiguration configuration)
    {
        var option = configuration.Get<AutoMapperOption>();

        var assemblies = GetAssemblies(option.AssemblyNamesForLoadProfiles);

        return services.AddAutoMapper(assemblies).AddSingleton<IMapperAdapter, AutoMapperAdapter>();
    }

    public static IServiceCollection AddBaseAutoMapperProfiles(this IServiceCollection services, Action<AutoMapperOption> setupAction)
    {
        var option = new AutoMapperOption();
        setupAction.Invoke(option);

        var assemblies = GetAssemblies(option.AssemblyNamesForLoadProfiles);

        return services.AddAutoMapper(assemblies).AddSingleton<IMapperAdapter, AutoMapperAdapter>();
    }

    private static List<Assembly> GetAssemblies(string assemblyNames)
    {
        var dependencies = DependencyContext.Default.RuntimeLibraries;

        return (from library in dependencies where IsCandidateCompilationLibrary(library, assemblyNames.Split(',')) select Assembly.Load(new AssemblyName(library.Name))).ToList();
    }

    private static bool IsCandidateCompilationLibrary(RuntimeLibrary compilationLibrary, string[] assemblyNames)
        => assemblyNames.Any(d => compilationLibrary.Name.Contains(d))
           || compilationLibrary.Dependencies.Any(d => assemblyNames.Any(c => d.Name.Contains(c)));
}
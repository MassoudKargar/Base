namespace Base.Extensions.DependencyInjection.Sample.Services;

public class GetGuidScopeService : IGetGuidScopeService
{
    private Guid guid { get; set; } = Guid.NewGuid();
    public Guid Execute() => guid;
}

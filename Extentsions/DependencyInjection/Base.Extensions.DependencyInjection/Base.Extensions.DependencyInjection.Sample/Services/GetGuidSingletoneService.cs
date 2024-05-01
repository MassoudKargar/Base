namespace Base.Extensions.DependencyInjection.Sample.Services;

public class GetGuidSingletonService : IGetGuidSingletonService
{
    private Guid guid { get; set; }

    public GetGuidSingletonService()
    {
        guid = Guid.NewGuid();
    }

    public Guid Execute() => guid;
}

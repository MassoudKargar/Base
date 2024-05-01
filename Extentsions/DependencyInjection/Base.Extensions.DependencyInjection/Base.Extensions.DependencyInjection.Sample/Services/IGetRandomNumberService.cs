namespace Base.Extensions.DependencyInjection.Sample.Services;

public interface IGetGuidTransientService : ITransientLifetime
{
    Guid Execute();
}
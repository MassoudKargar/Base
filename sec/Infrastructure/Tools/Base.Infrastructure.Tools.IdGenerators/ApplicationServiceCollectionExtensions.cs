using Base.Core.Domains.Contracts.Common;
using Base.Utility.Cryptography;

namespace Base.Infrastructure.Tools.IdGenerators;
public static class ApplicationServiceCollectionExtensions
{
    public static void AddIdGeneratorServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IIdGenerator, SnowflakeIdGenerator>();

        services.AddSingleton(sp =>
        {
            var options = new IdGeneratorOptions(sequenceOverflowStrategy: SequenceOverflowStrategy.SpinWait);
            int maxNumber = 1 << options.IdStructure.GeneratorIdBits;
            var workerId = WorkerIdGenerator.GenerateWorkerId(maxNumber);
            return new IdGenerator(workerId, options);
        });
    }
}
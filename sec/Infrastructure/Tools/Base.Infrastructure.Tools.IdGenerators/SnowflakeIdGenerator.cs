using Base.Core.Domains.Contracts.Common;

namespace Base.Infrastructure.Tools.IdGenerators;

internal class SnowflakeIdGenerator(IdGenerator idGenerator) : IIdGenerator
{
    public long GetNewIdLong()
    {
        return idGenerator.CreateId();
    }
}
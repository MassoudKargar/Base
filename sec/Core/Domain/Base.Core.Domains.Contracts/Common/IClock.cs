namespace Base.Core.Domains.Contracts.Common;

public interface IClock
{
    DateTimeOffset Now();
    void SetDate(DateTimeOffset? dateTimeOffset);
}
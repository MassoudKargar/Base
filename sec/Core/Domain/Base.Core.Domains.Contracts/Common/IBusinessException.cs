namespace Base.Core.Domains.Contracts.Common;

public interface IBusinessException
{
    string? GetCode();
    string GetMessage();
}

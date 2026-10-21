using Base.Core.Domains.Contracts.Common;

namespace Base.Core.Domains;

public class BusinessException : Exception, IBusinessException
{
    public virtual string? Code { get; private set; }

    public BusinessException()
    { }


    public BusinessException(string? message = null, string? code = null)
         : base(message)
    {
        Code = code;
    }

    public BusinessException(string message, Exception innerException)
        : base(message, innerException)
    { }

    public BusinessException(string message, string code, Exception innerException)
    : base(message, innerException)
    {
        Code = code;
    }

    public string? GetCode()
    {
        return Code;
    }

    public string GetMessage()
    {
        return Message;
    }
}

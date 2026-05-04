namespace Base.Core.Application.Exceptions;

public class AccessException : AppException
{
    public AccessException()
        : base(ApiResultStatusCode.Unused)
    {
    }

    public AccessException(string message)
        : base(ApiResultStatusCode.Unused, message)
    {
    }

    public AccessException(object additionalData)
        : base(ApiResultStatusCode.Unused, additionalData)
    {
    }

    public AccessException(string message, object additionalData)
        : base(ApiResultStatusCode.Unused, message, additionalData)
    {
    }

    public AccessException(string message, Exception exception)
        : base(ApiResultStatusCode.Unused, message, exception)
    {
    }

    public AccessException(string message, Exception exception, object additionalData)
        : base(ApiResultStatusCode.Unused, message, exception, additionalData)
    {
    }
}
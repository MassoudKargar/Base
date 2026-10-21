namespace Base.Core.Application.Exceptions;

using System;

public class UnauthorizedException : AppException
{
    public UnauthorizedException()
        : base(ApiResultStatusCode.Unauthorized)
    {
    }

    public UnauthorizedException(string message)
        : base(ApiResultStatusCode.Unauthorized, message)
    {
    }

    public UnauthorizedException(object additionalData)
        : base(ApiResultStatusCode.Unauthorized, additionalData)
    {
    }

    public UnauthorizedException(string message, object additionalData)
        : base(ApiResultStatusCode.Unauthorized, message, additionalData)
    {
    }

    public UnauthorizedException(string message, Exception exception)
        : base(ApiResultStatusCode.Unauthorized, message, exception)
    {
    }

    public UnauthorizedException(string message, Exception exception, object additionalData)
        : base(ApiResultStatusCode.Unauthorized, message, exception, additionalData)
    {
    }
}
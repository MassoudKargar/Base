namespace Base.Core.Application.Exceptions;

public class DatabaseExceptions : AppException
{
    public DatabaseExceptions()
        : base(ApiResultStatusCode.ServiceUnavailable)
    {
    }
    public DatabaseExceptions(ApiResultStatusCode apiResultStatusCode)
        : base(apiResultStatusCode)
    {
    }

    public DatabaseExceptions(string message)
        : base(ApiResultStatusCode.ServiceUnavailable, message)
    {
    }

    public DatabaseExceptions(object additionalData)
        : base(ApiResultStatusCode.ServiceUnavailable, additionalData)
    {
    }

    public DatabaseExceptions(string message, object additionalData)
        : base(ApiResultStatusCode.ServiceUnavailable, message, additionalData)
    {
    }

    public DatabaseExceptions(string message, Exception exception)
        : base(ApiResultStatusCode.ServiceUnavailable, message, exception)
    {
    }

    public DatabaseExceptions(string message, Exception exception, object additionalData)
        : base(ApiResultStatusCode.ServiceUnavailable, message, exception, additionalData)
    {
    }
}
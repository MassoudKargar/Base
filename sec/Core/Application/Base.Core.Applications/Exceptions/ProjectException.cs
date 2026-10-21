namespace Base.Core.Application.Exceptions;

using System;
using System.Net;

public class ProjectException : AppException
{
    public new HttpStatusCode HttpStatusCode { get; set; }
    public new ApiResultStatusCode ApiStatusCode { get; set; }
    public new object AdditionalData { get; set; }

    public ProjectException()
        : this(ApiResultStatusCode.NotAcceptable)
    {
    }

    public ProjectException(ApiResultStatusCode statusCode)
        : this(statusCode, null)
    {
    }

    public ProjectException(string message)
        : this(ApiResultStatusCode.NotAcceptable, message)
    {
    }

    public ProjectException(ApiResultStatusCode statusCode, string message)
        : this(statusCode, message, HttpStatusCode.NotAcceptable)
    {
    }

    public ProjectException(string message, object additionalData)
        : this(ApiResultStatusCode.NotAcceptable, message, additionalData)
    {
    }

    public ProjectException(ApiResultStatusCode statusCode, object additionalData)
        : this(statusCode, null, additionalData)
    {
    }

    public ProjectException(ApiResultStatusCode statusCode, string message, object additionalData)
        : this(statusCode, message, HttpStatusCode.NotAcceptable, additionalData)
    {
    }

    public ProjectException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode)
        : this(statusCode, message, httpStatusCode, null)
    {
    }

    public ProjectException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, object additionalData)
        : this(statusCode, message, httpStatusCode, null, additionalData)
    {
    }

    public ProjectException(string message, Exception exception)
        : this(ApiResultStatusCode.NotAcceptable, message, exception)
    {
    }

    public ProjectException(string message, Exception exception, object additionalData)
        : this(ApiResultStatusCode.NotAcceptable, message, exception, additionalData)
    {
    }

    public ProjectException(ApiResultStatusCode statusCode, string message, Exception exception)
        : this(statusCode, message, HttpStatusCode.NotAcceptable, exception)
    {
    }

    public ProjectException(ApiResultStatusCode statusCode, string message, Exception exception, object additionalData)
        : this(statusCode, message, HttpStatusCode.NotAcceptable, exception, additionalData)
    {
    }

    public ProjectException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, Exception exception)
        : this(statusCode, message, httpStatusCode, exception, null)
    {
    }

    public ProjectException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, Exception exception, object additionalData)
        : base(message, exception)
    {
        ApiStatusCode = statusCode;
        HttpStatusCode = httpStatusCode;
        AdditionalData = additionalData;
    }
}
using Base.Core.Application.Exceptions;
using Microsoft.OpenApi.Extensions;
namespace Base.EndPoints.Web.Results;

public class ApiResult(bool isSuccess, ApiResultStatusCode statusCode, string? message = null)
{
    public bool IsSuccess { get; set; } = isSuccess;
    public ApiResultStatusCode StatusCode { get; set; } = statusCode;
    public string Message { get; set; } = message ?? statusCode.GetDisplayName();

    #region Implicit Operators
    public static implicit operator ApiResult(OkResult _) => new(true, ApiResultStatusCode.OK);

    public static implicit operator ApiResult(BadRequestResult _) => new(false, ApiResultStatusCode.BadRequest);

    public static implicit operator ApiResult(BadRequestObjectResult result)
    {
        var message = result.Value?.ToString();
        if (result.Value is SerializableError errors)
        {
            var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
            message = string.Join(" | ", errorMessages);
        }
        return new ApiResult(false, ApiResultStatusCode.BadRequest, message);
    }

    public static implicit operator ApiResult(ContentResult result)
    => new(true, ApiResultStatusCode.OK, result.Content);

    public static implicit operator ApiResult(NotFoundResult _)
    => new(false, ApiResultStatusCode.NotFound);
    #endregion
}

public class ApiResult<TData>(bool isSuccess, ApiResultStatusCode statusCode, TData data, string? message = null)
    : ApiResult(isSuccess, statusCode, message)
    where TData : class
{
    public TData Data { get; set; } = data;

    //Schema = JsonSchema.FromType(data?.GetType());

    #region Implicit Operators
    public static implicit operator ApiResult<TData>(TData data)
        => new(true, ApiResultStatusCode.OK, data);

    public static implicit operator ApiResult<TData>(OkResult _)
    => new(true, ApiResultStatusCode.OK, null);

    public static implicit operator ApiResult<TData>(OkObjectResult result)
    => new(true, ApiResultStatusCode.OK, (TData)result.Value);

    public static implicit operator ApiResult<TData>(BadRequestResult _)
    => new(false, ApiResultStatusCode.BadRequest, null);

    public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
    {
        var message = result.Value?.ToString();
        if (result.Value is SerializableError errors)
        {
            var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
            message = string.Join(" | ", errorMessages);
        }
        return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null, message);
    }

    public static implicit operator ApiResult<TData>(ContentResult result)
    => new(true, ApiResultStatusCode.OK, null, result.Content);

    public static implicit operator ApiResult<TData>(NotFoundResult _)
    => new(false, ApiResultStatusCode.NotFound, null);

    public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
    => new(false, ApiResultStatusCode.NotFound, (TData)result.Value);
    #endregion
}

using System.Text.Json;
using Base.Core.Application.Exceptions;
using Base.EndPoints.Web.Results;

namespace Base.EndPoints.Web.Middlewares;

public static class CustomExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}

public class CustomExceptionHandlerMiddleware(
    RequestDelegate next,
    IWebHostEnvironment env,
    ILogger<CustomExceptionHandlerMiddleware> logger)
{
    private RequestDelegate Next { get; } = next;
    private IWebHostEnvironment Env { get; } = env;
    private ILogger<CustomExceptionHandlerMiddleware> Logger { get; } = logger;

    public async Task Invoke(HttpContext context)
    {
        string message = null;
        HttpStatusCode httpStatusCode = HttpStatusCode.ServiceUnavailable;
        ApiResultStatusCode apiStatusCode = ApiResultStatusCode.ServiceUnavailable;

        try
        {
            await Next(context);
            //WriteToLogger(new AppException());
        }
        catch (TimeoutException exception)
        {
            SetResponse(exception, HttpStatusCode.RequestTimeout, ApiResultStatusCode.RequestTimeout);
            await WriteToResponseAsync();
        }
        catch (TaskCanceledException exception)
        {
            SetResponse(exception, HttpStatusCode.GatewayTimeout, ApiResultStatusCode.GatewayTimeout);
            await WriteToResponseAsync();
        }
        catch (OperationCanceledException exception)
        {
            SetResponse(exception, HttpStatusCode.Gone, ApiResultStatusCode.Gone);
            await WriteToResponseAsync();
        }
        catch (NotFoundException exception)
        {
            SetResponse(exception, HttpStatusCode.NotFound, ApiResultStatusCode.NotFound);
            await WriteToResponseAsync();
        }
        catch (LogicException exception)
        {
            SetResponse(exception, HttpStatusCode.Conflict, ApiResultStatusCode.Conflict);
            await WriteToResponseAsync();
        }
        catch (SqlException exception)
        {
            SetResponse(exception, HttpStatusCode.ServiceUnavailable, ApiResultStatusCode.ServiceUnavailable);
            await WriteToResponseAsync();
        }
        catch (DatabaseExceptions exception)
        {
            SetResponse(exception, exception.HttpStatusCode, exception.ApiStatusCode);
            await WriteToResponseAsync();
        }
        catch (BadRequestException exception)
        {
            SetResponse(exception, HttpStatusCode.BadRequest, ApiResultStatusCode.BadRequest);
            await WriteToResponseAsync();
        }
        catch (UnauthorizedException exception)
        {
            SetResponse(exception, HttpStatusCode.Unauthorized, ApiResultStatusCode.Unauthorized);
            await WriteToResponseAsync();
        }
        catch (SecurityTokenExpiredException exception)
        {
            SetResponse(exception, HttpStatusCode.ExpectationFailed, ApiResultStatusCode.ExpectationFailed);
            await WriteToResponseAsync();
        }
        catch (UnauthorizedAccessException exception)
        {
            SetResponse(exception, HttpStatusCode.Unused, ApiResultStatusCode.Unused);
            await WriteToResponseAsync();
        }
        catch (NullSampleException exception)
        {
            SetResponse(exception, HttpStatusCode.NotModified, ApiResultStatusCode.NullSampleException);
            await WriteToResponseAsync();
        }
        catch (AccessException exception)
        {
            SetResponse(exception, HttpStatusCode.NotAcceptable, ApiResultStatusCode.NotAcceptable);
            await WriteToResponseAsync();
        }
        catch (ProjectException exception)
        {
            SetResponse(exception, exception.HttpStatusCode, exception.ApiStatusCode);
            await WriteToResponseAsync();
        }
        catch (AppException exception)
        {
            SetResponse(exception, exception.HttpStatusCode, exception.ApiStatusCode);
            await WriteToResponseAsync();
        }

        catch (Exception exception)
        {
            if (Env.IsDevelopment())
            {
                Dictionary<string, string> dic = new()
                {
                    ["Exception"] = exception.Message,
                    ["StackTrace"] = exception.StackTrace,
                };

                message = JsonSerializer.Serialize(dic);
            }
            message = exception.Message;
            await WriteToResponseAsync();
        }
        finally
        {
        }

        async Task WriteToResponseAsync()
        {
            if (context.Response.HasStarted)
                throw new InvalidOperationException(
                    "پاسخ قبلا شروع شده است، میان افزار کد وضعیت http اجرا نخواهد شد.");
            context.Response.StatusCode = (int)httpStatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new ApiResult(false, apiStatusCode, message)));
        }

        void SetResponse(Exception exception, HttpStatusCode httpStatus, ApiResultStatusCode apiResultStatus)
        {
            httpStatusCode = httpStatus;
            apiStatusCode = apiResultStatus;

            if (Env.IsDevelopment())
            {
                var dic = new Dictionary<string, string>
                {
                    ["Exception"] = exception.Message,
                    ["StackTrace"] = exception.StackTrace
                };
                if (exception is SecurityTokenExpiredException tokenException)
                    dic.Add("Expires", tokenException.Expires.ToString());

                message = JsonSerializer.Serialize(dic);
            }

            message = exception.Message;
        }
    }
}

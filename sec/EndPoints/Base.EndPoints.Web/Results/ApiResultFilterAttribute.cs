using Base.Core.Application.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Base.EndPoints.Web.Results;
public class ApiResultFilterAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        switch (context.Result)
        {
            case OkObjectResult okObjectResult:
                {
                    ApiResult apiResult = new ApiResult<object>(true, ApiResultStatusCode.OK, okObjectResult.Value);
                    context.Result = new JsonResult(apiResult) { StatusCode = okObjectResult.StatusCode };
                    break;
                }
            case OkResult okResult:
                {
                    ApiResult apiResult = new(true, ApiResultStatusCode.OK);
                    context.Result = new JsonResult(apiResult) { StatusCode = okResult.StatusCode };
                    break;
                }
            //return BadRequest() method create an ObjectResult with StatusCode 400 in recent versions, So the following code has changed a bit.
            case ObjectResult { StatusCode: 400 } badRequestObjectResult:
                {
                    string message = null;
                    switch (badRequestObjectResult.Value)
                    {
                        case ValidationProblemDetails validationProblemDetails:
                            var errorMessages = validationProblemDetails.Errors.SelectMany(p => p.Value).Distinct();
                            message = string.Join(" | ", errorMessages);
                            break;
                        case SerializableError errors:
                            var errorMessages2 = errors.SelectMany(p => (string[])p.Value).Distinct();
                            message = string.Join(" | ", errorMessages2);
                            break;
                        case { } and not ProblemDetails:
                            message = badRequestObjectResult.Value.ToString();
                            break;
                    }

                    var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest, message);
                    context.Result = new JsonResult(apiResult) { StatusCode = badRequestObjectResult.StatusCode };
                    break;
                }
            case ObjectResult { StatusCode: 404 } notFoundObjectResult:
                {
                    string message = null;
                    if (notFoundObjectResult.Value != null && notFoundObjectResult.Value is not ProblemDetails)
                        message = notFoundObjectResult.Value.ToString();

                    //var apiResult = new ApiResult<object>(false, ApiResultStatusCode.NotFound, notFoundObjectResult.Value);
                    var apiResult = new ApiResult(false, ApiResultStatusCode.NotFound, message);
                    context.Result = new JsonResult(apiResult) { StatusCode = notFoundObjectResult.StatusCode };
                    break;
                }
            case ContentResult contentResult:
                {
                    var apiResult = new ApiResult(true, ApiResultStatusCode.OK, contentResult.Content);
                    context.Result = new JsonResult(apiResult) { StatusCode = contentResult.StatusCode };
                    break;
                }
            case ObjectResult { StatusCode: null, Value: not ApiResult } objectResult:
                {
                    var apiResult = new ApiResult<object>(true, ApiResultStatusCode.OK, objectResult.Value);
                    context.Result = new JsonResult(apiResult) { StatusCode = objectResult.StatusCode };
                    break;
                }
        }

        base.OnResultExecuting(context);
    }
}
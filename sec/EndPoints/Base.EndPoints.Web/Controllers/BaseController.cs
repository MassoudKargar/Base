using Base.EndPoints.Web.Results;
using MediatR;

namespace Base.EndPoints.Web.Controllers;

/// <summary>
/// کنترلر پایه برای مدیریت درخواست‌های API.
/// این کلاس تمام ویژگی‌ها و متدهای عمومی برای سایر کنترلرهای API را فراهم می‌آورد.
/// </summary>
[ApiController]
[ApiResultFilter]
[Route("/api/[controller]/[action]")]
public class BaseController(IMediator mediator) : ControllerBase
{
    protected IMediator Mediator = mediator;
}

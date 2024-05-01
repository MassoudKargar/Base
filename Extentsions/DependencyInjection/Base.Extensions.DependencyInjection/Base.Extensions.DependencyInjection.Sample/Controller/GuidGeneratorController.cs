using Microsoft.AspNetCore.Mvc;
using Base.Extensions.DependencyInjection.Sample.Services;

namespace Base.Extensions.DependencyInjection.Sample.Controller;

[Route("api/[controller]")]
[ApiController]
public class GuidGeneratorController(IGetGuidSingletonService getRandomNumberSingletonService) : ControllerBase
{
    private readonly IGetGuidSingletonService _getRandomNumberSingletonService = getRandomNumberSingletonService;

    [HttpGet("GetRandomNumberTransient")]
    public async Task<IActionResult> GetRandomNumberTransient([FromServices] IGetGuidTransientService service1,
                                                              [FromServices] IGetGuidTransientService service2)
        => Ok(string.Format("1 : {0} , 2 : {1}", service1.Execute(), service2.Execute()));

    [HttpGet("GetRandomNumberScope")]
    public async Task<IActionResult> GetRandomNumberScope([FromServices] IGetGuidScopeService service1,
                                                          [FromServices] IGetGuidScopeService service2)
        => Ok(string.Format("1 : {0} , 2 : {1}",
            service1.Execute(),
            service2.Execute()));

    [HttpGet("GetRandomNumberSingletone")]
    public async Task<IActionResult> GetRandomNumberSingletone([FromServices] IGetGuidSingletonService service1,
                                                               [FromServices] IGetGuidSingletonService service2)
        => Ok(string.Format("1 : {0} , 2 : {1}", service1.Execute(), service2.Execute()));
}

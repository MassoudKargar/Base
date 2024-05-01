using Microsoft.AspNetCore.Mvc;
using Base.Extensions.Translations.Abstractions;

namespace Base.Extensions.Translations.Parrot.Sample.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ParrotController(ITranslator translator) : ControllerBase
{
    [HttpGet(Name = "GetTranslation")]
    public IActionResult Get(string key)
    {
        return Ok(translator.GetString(key));
    }
}
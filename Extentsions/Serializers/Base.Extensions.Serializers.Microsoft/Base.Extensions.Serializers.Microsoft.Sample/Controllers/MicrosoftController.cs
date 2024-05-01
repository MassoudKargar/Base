using Microsoft.AspNetCore.Mvc;
using Base.Extensions.Serializers.Abstractions;
using Base.Extensions.Serializers.Microsoft.Sample.Models;

namespace Base.Extensions.Serializers.Microsoft.Sample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MicrosoftController : ControllerBase
{
    private readonly IJsonSerializer _jsonSerializer;

    public MicrosoftController(IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

    [HttpGet("Serilize")]
    public IActionResult Serilize([FromQuery] Person person) => Ok(_jsonSerializer.Serialize(person));

    [HttpGet("Deserialize")]
    public IActionResult Deserialize([FromQuery] string input) => Ok(_jsonSerializer.Deserialize<Person>(input));
}

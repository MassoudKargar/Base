namespace Base.Extensions.MessageBus.RabbitMQ.Sample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventAndCommandSampleController(ISendMessageBus sendMessageBus) : ControllerBase
{
    [HttpPost("SendEvent")]
    public IActionResult SendEvent([FromBody] PersonEvent personEvent)
    {
        sendMessageBus.Publish(personEvent);
        return Ok();
    }

    [HttpPost("SendCommand")]
    public IActionResult SendCommand([FromBody] PersonCommand personCommand)
    {
        sendMessageBus.SendCommandTo("SampleApplication", "PersonCommand", personCommand);
        return Ok();
    }
}
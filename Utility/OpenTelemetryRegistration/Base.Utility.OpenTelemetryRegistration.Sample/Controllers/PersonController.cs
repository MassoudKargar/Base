using System.Diagnostics;

namespace Base.Utilities.OpenTelemetryRegistration.Sample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly PersonContext context;
    private readonly ILogger<PersonController> logger;

    public PersonController(PersonContext context, ILogger<PersonController> logger)
    {
        this.context = context;
        this.logger = logger;
    }
    [HttpGet(Name = "GetPerson")]
    public async Task<ActionResult> Get()
    {
        return Ok(await context.People.ToListAsync());
    }
    [HttpPost(Name = "SavePerson")]

    public async Task<IActionResult> Save([FromBody] Person person)
    {
        using (logger.BeginScope("Request Recived at {path}", "SavePerson"))
        {
            using (var activity = new Activity("SavePerson").Start())
            {
                logger.LogDebug("person start");
                await context.People.AddAsync(person);
                await context.SaveChangesAsync();
                logger.LogDebug("person end");
            }
        }
        return Ok(person.Id);
    }
}
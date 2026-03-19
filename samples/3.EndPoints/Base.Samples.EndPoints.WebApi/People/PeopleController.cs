namespace Base.Samples.EndPoints.WebApi.People;

[Route("api/[controller]")]
public class PeopleController : BaseController
{
    #region Commands
    [HttpPost("Create")]
    public async Task<IActionResult> CreatePerson([FromBody] CreatePerson command, CancellationToken cancellationToken) =>
        await Create<CreatePerson, long>(command, cancellationToken);


    #endregion
}
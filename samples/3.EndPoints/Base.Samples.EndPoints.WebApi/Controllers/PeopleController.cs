using Base.Core.Domain.Exceptions;
using Base.Samples.Core.Domain.People.ValueObject;

using Microsoft.AspNetCore.Mvc;

namespace Base.Samples.EndPoints.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeopleController : ControllerBase
{
    [HttpGet("CheckValueObject")]
    public IActionResult CheckFirstNameValueObject(string firstName)
    {
        FirstName fname1 = new(firstName);
        FirstName fname2 = new(firstName);
        return Ok(fname1 == fname2);
    }

    [HttpGet("ExceptionCheck")]
    public IActionResult CheckFirstNameValueObject()
    {
        try
        {
            FirstName fname1 = new("1");
        }
        catch (InvalidValueObjectStateException e)
        {
            return Ok(e.ToString());
        }

        return BadRequest();
    }
}
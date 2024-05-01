using Microsoft.AspNetCore.Mvc;
using Base.Extensions.ObjectMappers.AutoMapper.Sample.Models;
using Base.Extensions.ObjectMappers.Abstractions;

namespace Base.Extensions.ObjectMappers.AutoMapper.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoMapperController(IMapperAdapter mapperAdapter) : ControllerBase
    {
        [HttpGet("MapPersonToStudent")]
        public IActionResult MapPersonToStudent([FromQuery] Person model)
        {
            var student = mapperAdapter.Map<Person, Student>(model);
            return Ok(student);
        }

        [HttpGet("MapStudentToPerson")]
        public IActionResult MapStudentToPerson([FromQuery] Student model)
        {
            var person = mapperAdapter.Map<Student, Person>(model);
            return Ok(person);
        }
    }
}

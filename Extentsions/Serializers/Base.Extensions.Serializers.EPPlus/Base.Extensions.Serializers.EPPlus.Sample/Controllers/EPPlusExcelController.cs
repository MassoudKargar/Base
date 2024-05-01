using Microsoft.AspNetCore.Mvc;
using Base.Extensions.Serializers.EPPlus.Sample.Models;
using Base.Extensions.Serializers.Abstractions;

namespace Base.Extensions.Serializers.EPPlus.Sample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EPPlusExcelController(IExcelSerializer excelSerializer) : ControllerBase
{
    [HttpPut("ToList")]
    public IActionResult ToList(IFormFile file)
    {
        if (file.Length > 0)
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();

                return Ok(excelSerializer.ExcelToList<ExcelModel>(fileBytes));
            }

        return Ok();
    }

    [HttpGet("ToExcel")]
    public IActionResult ToExcel([FromQuery] List<string> model)
    {
        if (model == null || !model.Any()) return Ok();

        var services = model.Select(s => new ExcelModel() { Title = s }).ToList();

        var result = excelSerializer.ListToExcelByteArray(services);

        return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "services.xlsx");
    }
}
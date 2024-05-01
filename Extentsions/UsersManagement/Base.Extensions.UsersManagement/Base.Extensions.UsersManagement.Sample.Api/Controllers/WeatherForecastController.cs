﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Base.Extensions.UsersManagement.Abstractions;
using Base.Extensions.UsersManagement.Sample.Api.Models;

namespace Base.Extensions.UsersManagement.Sample.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    public WeatherForecastController(IUserInfoService userInfoService)
    {
        var user = HttpContext.User;

        _userInfoService = userInfoService;
    }
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IUserInfoService _userInfoService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var res = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

        return res;
    }
}

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WasmHosted.Shared;

namespace WasmHosted.Server.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        string first = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value;
        string last = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname).Value;
        
        _logger.LogInformation("User requesting weather is: " + first + " " + last);
        
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}

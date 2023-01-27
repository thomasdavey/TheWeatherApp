using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TheWeatherApp.Core.V1.Models;
using TheWeatherApp.Core.V1.Service;

namespace TheWeatherApp.Api.V1.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _service;

    public WeatherController(ILogger<WeatherController> logger, IWeatherService service)
    {
        _service = service;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get([FromQuery] string location)
    {
        var response = await _service.GetWeather(new WeatherRequest() { Location = location });

        if (response != null)
        {
            return StatusCode((int)response.StatusCode, JsonSerializer.Serialize(response.Weather));
        }

        return StatusCode(500, null);
    }
}

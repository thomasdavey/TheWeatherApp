using System.Net;

namespace TheWeatherApp.Core.V1.Models;

public class WeatherResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public Weather? Weather { get; set; }
}
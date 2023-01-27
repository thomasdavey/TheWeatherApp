using TheWeatherApp.Core.V1.Models;

namespace TheWeatherApp.Core.V1.Service;

public interface IWeatherService
{
    public Task<WeatherResponse> GetWeather(WeatherRequest request);
}
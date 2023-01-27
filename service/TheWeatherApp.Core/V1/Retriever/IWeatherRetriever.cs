using TheWeatherApp.Core.V1.Models;

namespace TheWeatherApp.Core.V1.Retriever;

public interface IWeatherRetriever
{
    public Task<WeatherResponse> RetrieveWeather(WeatherRequest request);
}
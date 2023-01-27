using System.Net;
using TheWeatherApp.Core.V1.Models;
using TheWeatherApp.Core.V1.Retriever;

namespace TheWeatherApp.Core.V1.Service;

public class WeatherService : IWeatherService
{
    private readonly IWeatherRetriever _retriever;

    public WeatherService(IWeatherRetriever retriever)
    {
        _retriever = retriever;
    }
    
    public async Task<WeatherResponse> GetWeather(WeatherRequest request)
    {
        try
        {
            var weather = await _retriever.RetrieveWeather(request);

            return weather;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
            return new WeatherResponse()
            {
                StatusCode = HttpStatusCode.InternalServerError
            };
        }
    }
}
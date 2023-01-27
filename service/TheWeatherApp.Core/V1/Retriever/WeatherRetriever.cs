using System.Net;
using Microsoft.Extensions.Options;
using TheWeatherApp.Core.V1.Models;
using TheWeatherApp.Core.V1.Utilities;

namespace TheWeatherApp.Core.V1.Retriever;

public class WeatherRetriever : IWeatherRetriever
{
    private readonly string? _apiKey;
    private readonly HttpClient _client;

    private const string UriBase = "https://api.openweathermap.org/data/2.5/weather?";

    public WeatherRetriever(IOptions<WeatherOptions> options, HttpClient client)
    {
        _apiKey = options.Value.ApiKey;
        _client = client;
        
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
    }
    
    public async Task<WeatherResponse> RetrieveWeather(WeatherRequest request)
    {
        if (string.IsNullOrEmpty(request.Location) || string.IsNullOrEmpty(_apiKey))
        {
            return new WeatherResponse
            {
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        
        var uri = UriBase + $"q={request.Location}&appid={_apiKey}&units=metric";
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        var response = await _client.SendAsync(requestMessage);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return new WeatherResponse
            {
                StatusCode = response.StatusCode
            };
        }

        var weatherData = await response.Content.ReadAsStringAsync();
        var weather = WeatherFactory.JsonToWeather(weatherData);

        return new WeatherResponse
        {
            StatusCode = weather != null ? HttpStatusCode.OK : HttpStatusCode.InternalServerError,
            Weather = weather
        };
    }
}
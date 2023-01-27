using Newtonsoft.Json.Linq;
using TheWeatherApp.Core.V1.Models;

namespace TheWeatherApp.Core.V1.Utilities;

public static class WeatherFactory
{
    public static Weather? JsonToWeather(string json)
    {
        try
        {
            var jsonObject = JObject.Parse(json);

            var weather = new Weather
            {
                Location = new Location
                {
                    Name = jsonObject["name"].ToString(),
                    Country = jsonObject["sys"]["country"].ToString(),
                },
                Conditions = jsonObject["weather"][0]["main"].ToString(),
                Humidity = int.Parse(jsonObject["main"]["humidity"].ToString()),
                Pressure = int.Parse(jsonObject["main"]["pressure"].ToString()),
                Sunrise = DateTimeOffset.FromUnixTimeSeconds(long.Parse(jsonObject["sys"]["sunrise"].ToString()))
                    .DateTime.ToString("h:mm tt"),
                Sunset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(jsonObject["sys"]["sunset"].ToString())).DateTime
                    .ToString("h:mm tt"),
                Temperature = new Temperature
                {
                    Current = float.Parse(jsonObject["main"]["temp"].ToString()),
                    Minimum = float.Parse(jsonObject["main"]["temp_min"].ToString()),
                    Maximum = float.Parse(jsonObject["main"]["temp_max"].ToString()),
                    FeelsLike = float.Parse(jsonObject["main"]["feels_like"].ToString()),
                },
                Icon = jsonObject["weather"][0]["icon"].ToString()
            };

            return weather;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
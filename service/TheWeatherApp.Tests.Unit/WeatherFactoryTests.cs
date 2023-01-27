using FluentAssertions;
using TheWeatherApp.Core.V1.Models;
using TheWeatherApp.Core.V1.Utilities;
using Xunit;

namespace TheWeatherApp.Tests.Unit;

public class WeatherFactoryTests
{
    [Fact]
    public void InvalidJsonStringInput_ExceptionsCaughtAndReturnsNull()
    {
        var output = WeatherFactory.JsonToWeather("Testing...");

        output.Should().BeNull();
    }
    
    [Fact]
    public void MissingJsonValue_ExceptionsCaughtAndReturnsNull()
    {
        // Missing main value
        var output = WeatherFactory.JsonToWeather(
            "{\"coord\":{\"lon\":-0.5379,\"lat\":53.2268},\"weather\":[{\"id\":701,\"description\":\"mist\",\"icon\":\"50n\"}],\"base\":\"stations\",\"main\":{\"temp\":3.97,\"feels_like\":0.52,\"temp_min\":2.84,\"temp_max\":4.71,\"pressure\":1031,\"humidity\":93},\"visibility\":10000,\"wind\":{\"speed\":4.12,\"deg\":350},\"clouds\":{\"all\":100},\"dt\":1674785942,\"sys\":{\"type\":2,\"id\":2033635,\"country\":\"GB\",\"sunrise\":1674806088,\"sunset\":1674837275},\"timezone\":0,\"id\":2644487,\"name\":\"Lincoln\",\"cod\":200}");

        output.Should().BeNull();
    }

    [Fact]
    public void ValidJsonStringInput_ReturnsWeatherObject()
    {
        var output = WeatherFactory.JsonToWeather(
            "{\"coord\":{\"lon\":-0.5379,\"lat\":53.2268},\"weather\":[{\"id\":701,\"main\":\"Mist\",\"description\":\"mist\",\"icon\":\"50n\"}],\"base\":\"stations\",\"main\":{\"temp\":3.97,\"feels_like\":0.52,\"temp_min\":2.84,\"temp_max\":4.71,\"pressure\":1031,\"humidity\":93},\"visibility\":10000,\"wind\":{\"speed\":4.12,\"deg\":350},\"clouds\":{\"all\":100},\"dt\":1674785942,\"sys\":{\"type\":2,\"id\":2033635,\"country\":\"GB\",\"sunrise\":1674806088,\"sunset\":1674837275},\"timezone\":0,\"id\":2644487,\"name\":\"Lincoln\",\"cod\":200}");

        output.Should().NotBeNull();
        output.Should().BeEquivalentTo(new Weather
        {
            Conditions = "Mist",
            Humidity = 93,
            Icon = "50n",
            Location = new Location
            {
                Country = "GB",
                Name = "Lincoln"
            },
            Pressure = 1031,
            Sunrise = "7:54 am",
            Sunset = "4:34 pm",
            Temperature = new Temperature
            {
                Current = 3.97F,
                FeelsLike = 0.52F,
                Maximum = 4.71F,
                Minimum = 2.84F
            }
        });
    }

}
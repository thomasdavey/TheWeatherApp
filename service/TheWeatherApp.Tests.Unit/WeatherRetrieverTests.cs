using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using TheWeatherApp.Core.V1.Models;
using TheWeatherApp.Core.V1.Retriever;
using Xunit;

namespace TheWeatherApp.Tests.Unit;

public class WeatherRetrieverTests
{
    [Theory]
    [InlineData("Key", null)]
    [InlineData(null, "Location")]
    [InlineData("Key", "")]
    [InlineData("", "Location")]
    [InlineData("", "")]
    [InlineData(null, null)]
    public void RequestMadeWithMissingApiKeyOrLocation_BadRequestStatusWeatherResponseIsReturned(string apiKey, string location)
    {
        var options = new WeatherOptions() { ApiKey = apiKey };
        var mockOptions = new Mock<IOptions<WeatherOptions>>();
        mockOptions.Setup(x => x.Value).Returns(options);

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\"coord\":{\"lon\":-0.5379,\"lat\":53.2268},\"weather\":[{\"id\":701,\"main\":\"Mist\",\"description\":\"mist\",\"icon\":\"50n\"}],\"base\":\"stations\",\"main\":{\"temp\":3.97,\"feels_like\":0.52,\"temp_min\":2.84,\"temp_max\":4.71,\"pressure\":1031,\"humidity\":93},\"visibility\":10000,\"wind\":{\"speed\":4.12,\"deg\":350},\"clouds\":{\"all\":100},\"dt\":1674785942,\"sys\":{\"type\":2,\"id\":2033635,\"country\":\"GB\",\"sunrise\":1674806088,\"sunset\":1674837275},\"timezone\":0,\"id\":2644487,\"name\":\"Lincoln\",\"cod\":200}")
        };

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);

        var client = new HttpClient(mockHttpMessageHandler.Object);
        
        var retriever = new WeatherRetriever(mockOptions.Object, client);

        var output = retriever.RetrieveWeather(new WeatherRequest { Location = location });

        output.Result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public void InvalidRequestMadeToRetrieveWeather_WeatherResponseIsReturnedWithoutOkStatus()
    {
        var options = new WeatherOptions() { ApiKey = "Key" };
        var mockOptions = new Mock<IOptions<WeatherOptions>>();
        mockOptions.Setup(x => x.Value).Returns(options);

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.NotFound,
            Content = new StringContent("Not Found")
        };

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);

        var client = new HttpClient(mockHttpMessageHandler.Object);
        
        var retriever = new WeatherRetriever(mockOptions.Object, client);

        var output = retriever.RetrieveWeather(new WeatherRequest { Location = "Test" });

        output.Result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        output.Result.Weather.Should().BeNull();
    }
    
    [Fact]
    public void HttpClientReturnsInvalidJson_WeatherResponseIsReturnedWithoutOkStatus()
    {
        var options = new WeatherOptions() { ApiKey = "Key" };
        var mockOptions = new Mock<IOptions<WeatherOptions>>();
        mockOptions.Setup(x => x.Value).Returns(options);

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{}")
        };

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);

        var client = new HttpClient(mockHttpMessageHandler.Object);
        
        var retriever = new WeatherRetriever(mockOptions.Object, client);

        var output = retriever.RetrieveWeather(new WeatherRequest { Location = "Test" });

        output.Result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        output.Result.Weather.Should().BeNull();
    }
    
    [Fact]
    public void ValidRequestMadeToRetrieveWeather_WeatherResponseIsReturnedWithOkStatus()
    {
        var options = new WeatherOptions() { ApiKey = "Key" };
        var mockOptions = new Mock<IOptions<WeatherOptions>>();
        mockOptions.Setup(x => x.Value).Returns(options);

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\"coord\":{\"lon\":-0.5379,\"lat\":53.2268},\"weather\":[{\"id\":701,\"main\":\"Mist\",\"description\":\"mist\",\"icon\":\"50n\"}],\"base\":\"stations\",\"main\":{\"temp\":3.97,\"feels_like\":0.52,\"temp_min\":2.84,\"temp_max\":4.71,\"pressure\":1031,\"humidity\":93},\"visibility\":10000,\"wind\":{\"speed\":4.12,\"deg\":350},\"clouds\":{\"all\":100},\"dt\":1674785942,\"sys\":{\"type\":2,\"id\":2033635,\"country\":\"GB\",\"sunrise\":1674806088,\"sunset\":1674837275},\"timezone\":0,\"id\":2644487,\"name\":\"Lincoln\",\"cod\":200}")
        };

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);

        var client = new HttpClient(mockHttpMessageHandler.Object);
        
        var retriever = new WeatherRetriever(mockOptions.Object, client);

        var output = retriever.RetrieveWeather(new WeatherRequest { Location = "Test" });

        output.Result.StatusCode.Should().Be(HttpStatusCode.OK);
        output.Result.Weather.Should().NotBeNull();
        output.Result.Weather.Should().BeEquivalentTo(new Weather
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
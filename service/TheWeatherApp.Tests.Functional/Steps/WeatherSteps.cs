using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using TheWeatherApp.Api;
using TheWeatherApp.Core.V1.Models;
using TheWeatherApp.Core.V1.Retriever;
using Xunit;

namespace TheWeatherApp.Tests.Functional.Steps;

[Binding]
public sealed class WeatherSteps : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private string _location;
    private HttpResponseMessage _response;

    public WeatherSteps(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Given(@"A valid input weather request")]
    public void GivenAValidInputWeatherRequest()
    {
        _location = "Lincoln, UK";
    }

    [When(@"A request is made to the weather service")]
    public void WhenARequestIsMadeToTheWeatherService()
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

        var retrieverClient = new HttpClient(mockHttpMessageHandler.Object);
        
        var retriever = new WeatherRetriever(mockOptions.Object, retrieverClient);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = new ServiceDescriptor(typeof(IWeatherRetriever), retriever);
                services.Replace(descriptor);
            });
        }).CreateClient();

        _response = client.GetAsync($"api/v1/Weather?location={_location}").Result;
    }

    [Then(@"a 200 response and the weather is returned")]
    public void ThenAResponseAndTheWeatherIsReturned()
    {
        _response.StatusCode.Should().Be(HttpStatusCode.OK);
        JsonSerializer.Deserialize<Weather>(_response.Content.ReadAsStringAsync().Result).Should().BeEquivalentTo(new Weather
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
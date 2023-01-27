using Microsoft.Extensions.DependencyInjection;
using TheWeatherApp.Core.V1.Retriever;
using TheWeatherApp.Core.V1.Service;

namespace TheWeatherApp.Core.V1;

public static class Dependencies
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {
        services.AddTransient<IWeatherService, WeatherService>();
        services.AddTransient<IWeatherRetriever, WeatherRetriever>();
        
        return services;
    }
}
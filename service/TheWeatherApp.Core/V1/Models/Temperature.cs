namespace TheWeatherApp.Core.V1.Models;

public class Temperature
{
    public float Current { get; set; }
    public float Minimum { get; set; }
    public float Maximum { get; set; }
    public float FeelsLike { get; set; }
}
namespace TheWeatherApp.Core.V1.Models;

public class Weather
{
    public Location Location { get; set; }
    public string Conditions { get; set; }
    public Temperature Temperature { get; set; }
    public int Humidity { get; set; }
    public int Pressure { get; set; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }
    public string Icon { get; set; }
}
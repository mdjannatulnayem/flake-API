using System.ComponentModel.DataAnnotations;

namespace flake_API.Models;

public class WeatherDataModel
{
    [Key]
    public DateTime Time { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double Pressure { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public LocationModel Location { get; set; } = null!;
}

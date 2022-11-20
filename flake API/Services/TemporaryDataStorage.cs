using flake_API.Models;
using flake_API.Models.dtoModels;

namespace flake_API.Services
{
    public static class TemporaryDataStorage
    {
        public static Dictionary<string, List<WeatherDTOModel>> WeatherData = new Dictionary<string, List<WeatherDTOModel>> {
            ["dhaka"] = new List<WeatherDTOModel> {
                new WeatherDTOModel{
                    Time = DateTime.Now,
                    Temperature = 31.53,
                    Humidity = 41,
                    Pressure = 101325.75,
                    Latitude = 23.777176,
                    Longitude = 90.399452
                }
            },
            ["mymensingh"] = new List<WeatherDTOModel> { 
                new WeatherDTOModel{ 
                    Time = DateTime.Now,
                    Temperature = 28.73,
                    Humidity = 33,
                    Pressure = 101325.73,
                    Latitude = 24.743448,
                    Longitude = 90.398384
                }
            },
            ["rajshahi"] = new List<WeatherDTOModel> {
                new WeatherDTOModel{
                    Time = DateTime.Now,
                    Temperature = 28.56,
                    Humidity = 31,
                    Pressure = 101325.45,
                    Latitude = 24.006355,
                    Longitude = 89.249298
                }
            },
            ["dinajpur"] = new List<WeatherDTOModel> {
                new WeatherDTOModel{
                    Time = DateTime.Now,
                    Temperature = 26.15,
                    Humidity = 30,
                    Pressure = 101325.63,
                    Latitude = 25.627912,
                    Longitude = 88.633176
                }
            },
            ["barisal"] = new List<WeatherDTOModel> {
                new WeatherDTOModel{
                    Time = DateTime.Now,
                    Temperature = 29.4,
                    Humidity = 35,
                    Pressure = 101325.80,
                    Latitude = 22.702921,
                    Longitude = 90.346597
                }
            },
            ["rangpur"] = new List<WeatherDTOModel> {
                new WeatherDTOModel{
                    Time = DateTime.Now,
                    Temperature = 28.0,
                    Humidity = 40,
                    Pressure = 101325.51,
                    Latitude = 25.744860,
                    Longitude = 89.275589
                }
            },
            ["sylhet"] = new List<WeatherDTOModel> {
                new WeatherDTOModel{
                    Time = DateTime.Now,
                    Temperature = 28.5,
                    Humidity = 37,
                    Pressure = 101325.66,
                    Latitude = 24.904539,
                    Longitude = 91.870722
                }
            },
            ["chittagong"] = new List<WeatherDTOModel> {
                new WeatherDTOModel{
                    Time = DateTime.Now,
                    Temperature = 30.5,
                    Humidity = 54,
                    Pressure = 101325.33,
                    Latitude = 22.341900,
                    Longitude = 91.815536
                }
            }
        };
    }
}

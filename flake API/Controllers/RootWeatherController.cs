using flake_API.Models;
using flake_API.Models.dtoModels;
using flake_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace flake_API.Controllers
{
    [ApiController]
    // [Route("flake/[controller]")]
    [Route("flake/api")]

    public class RootWeatherController : ControllerBase
    {

        // <<API endpoints>>

        // Test endpoint
        [HttpGet("index", Name = "Greetings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> Index() {
            return Ok("Greetings from flake weather service!");
        }

        // Get LATEST weather data for a specific location
        [HttpGet("{location}", Name = "GetCurrentData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<WeatherDTOModel> GetCurrentWeatherData(string location)
        {
            location = location.ToLower();
            if (!TemporaryDataStorage.WeatherData.ContainsKey(location))
                return NotFound();
            return Ok(TemporaryDataStorage.WeatherData[location][TemporaryDataStorage.WeatherData[location].Count - 1]);
        }


        // Get LATEST weather data upto a specific count
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{location}/{count:int}",Name = "GetCurrentDataUptoCount")]
        public ActionResult<IEnumerable<WeatherDTOModel>> GetCurrentWeatherDataUptoCount(string location,int count) {
            location = location.ToLower();
            if (!TemporaryDataStorage.WeatherData.ContainsKey(location))
                return NotFound();
            if (count <= 0) return BadRequest($"Count:{count} should be greater than zero!");
            var x = TemporaryDataStorage.WeatherData[location].Count;
            count = count > x ? x % count : count;
            return Ok(TemporaryDataStorage.WeatherData[location]
                .ToList<WeatherDTOModel>().GetRange(x - count,count));
        }


        [HttpGet("{location}/{date:DateTime}",Name = "GetDataFromDate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<WeatherDTOModel>> GetWeatherDataFromDate(string location,DateTime date)
        {
            location = location.ToLower();
            if (!TemporaryDataStorage.WeatherData.ContainsKey(location))
                return NotFound();
            if (date > DateTime.Today)
                return BadRequest("Invalid date arguement!");
            return Ok(TemporaryDataStorage.WeatherData[location]
                .ToList<WeatherDTOModel>().Where(x => x.Time >= date || x.Time < date.AddDays(1)));
        }


        // Get ALL weather data for a specific location
        [HttpGet("all/{location}",Name = "GetAllData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<WeatherDTOModel>> GetAllWeatherData(string location)
        {
            location = location.ToLower();
            if (!TemporaryDataStorage.WeatherData.ContainsKey(location))
                return NotFound();
            return Ok(TemporaryDataStorage.WeatherData[location]
                .ToList<WeatherDTOModel>().Where<WeatherDTOModel>(x => x.Time >= DateTime.Today));
        }


        // Post LATEST weather data for a specific location
        [HttpPost("{location}",Name = "StoreNewData")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<WeatherDTOModel> StoreNewData([FromBody]WeatherDTOModel dataModel,string location)
        {
            if (!TemporaryDataStorage.WeatherData.ContainsKey(location))
                return BadRequest($"{location} is invalid!");
            if (dataModel is null)
                return BadRequest();
            TemporaryDataStorage.WeatherData[location].Add(dataModel);
            return CreatedAtRoute("GetCurrentData", new { location = location} ,dataModel);
        }

        // <<API endpoints>>
    }
}

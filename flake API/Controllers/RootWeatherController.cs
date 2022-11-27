using AutoMapper;
using flake_API.Models;
using flake_API.Models.dtoModels;
using flake_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace flake_API.Controllers;

[ApiController]
[Route("flake/api")]
public class RootWeatherController : ControllerBase
{
    private readonly ApplicationDbContext _dbcontext;

    private readonly ILogger<RootWeatherController> _logger;

    private readonly IMapper _mapper;

    public RootWeatherController(ILogger<RootWeatherController> logger, ApplicationDbContext dbcontext,IMapper mapper)
    {
        _logger = logger;
        _dbcontext = dbcontext;
        _mapper = mapper;
    }

    // <<API endpoints>>

        
    // Get LATEST weather data for a specific location
    [HttpGet("{location}", Name = "GetCurrentData")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<WeatherDTOModel> GetCurrentWeatherData(string location)
    {
        location = location.ToLower();
        var _location = _dbcontext.Location
                    .Where(u => u.State == location)
                    .FirstOrDefault();
        if (_location is null)
        {
            _logger.LogWarning("____Attempted invalid location!");
            return NotFound();
        }
        if (_dbcontext.Weather.ToList().Count == 0)
        {
            _logger.LogError("____Data repository currently empty!");
            return NoContent();
        }
        var data = _mapper
                .Map<WeatherDTOModel>(_dbcontext.Weather
                .Where(u => u.Location == new LocationModel {
                    State = location
                }).OrderBy(u => u.Time).LastOrDefault());
        return Ok(data);
    }


    // Get LATEST weather data upto a specific count
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{location}/{count:int}", Name = "GetCurrentDataUptoCount")]
    public ActionResult<IEnumerable<WeatherDTOModel>> GetCurrentWeatherDataUptoCount(string location, int count)
    {
        location = location.ToLower();
        var _location = _dbcontext.Location
                    .Where(u => u.State == location)
                    .FirstOrDefault();
        if (_location is null)
        {
            _logger.LogWarning("____Attempted invalid location!");
            return NotFound();
        }
        if (_dbcontext.Weather.ToList().Count == 0)
        {
            _logger.LogError("____Data repository currently empty!");
            return NoContent();
        }
        if (count <= 0) return BadRequest($"Count:{count} should be greater than zero!");
        var x = _dbcontext.Weather
            .Where(u => u.Location == new LocationModel
            {
                State = location
            }).ToList().Count;
        count = count > x ? x % count : count;
        var data = _dbcontext.Weather
                .Where(u => u.Location == new LocationModel
                {
                    State = location
                }).ToList().GetRange(x - count, count);
        List<WeatherDTOModel> list = new();
        foreach(WeatherDataModel model in data)
        {
            list.Add(_mapper.Map<WeatherDTOModel>(model));
        }
        return Ok(list);
    }


    [HttpGet("{location}/{date:DateTime}", Name = "GetDataFromDate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<WeatherDTOModel>> GetWeatherDataFromDate(string location, DateTime date)
    {
        location = location.ToLower();
        var _location = _dbcontext.Location
                    .Where(u => u.State == location)
                    .FirstOrDefault();
        if (_location is null)
        {
            _logger.LogWarning("____Attempted invalid location!");
            return NotFound();
        }
        if (_dbcontext.Weather.ToList().Count == 0)
        {
            _logger.LogError("____Data repository currently empty!");
            return NoContent();
        }
        if (date > DateTime.Today) return BadRequest("Invalid date arguement!");
        var data = _dbcontext.Weather
                .Where(u => u.Location == new LocationModel
                {
                    State = location
                })
                .Where(u => u.Time >= date || u.Time < date.AddDays(1)).ToList();
        List<WeatherDTOModel> list = new();
        foreach (WeatherDataModel model in data)
        {
            list.Add(_mapper.Map<WeatherDTOModel>(model));
        }
        return Ok(list);
    }


    // Get ALL weather data for a specific location
    [HttpGet("all/{location}", Name = "GetAllData")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<WeatherDTOModel>> GetAllWeatherData(string location)
    {
        location = location.ToLower();
        var _location = _dbcontext.Location
                    .Where(u => u.State == location)
                    .FirstOrDefault();
        if (_location is null)
        {
            _logger.LogWarning("____Attempted invalid location!");
            return NotFound();
        }
        if (_dbcontext.Weather.ToList().Count == 0)
        {
            _logger.LogError("____Data repository currently empty!");
            return NoContent();
        }
        var data = _dbcontext.Weather
                .Where(u => u.Location == new LocationModel
                {
                    State = location
                })
                .Where(u => u.Time >= DateTime.Today).ToList();
        List<WeatherDTOModel> list = new();
        foreach (WeatherDataModel model in data)
        {
            list.Add(_mapper.Map<WeatherDTOModel>(model));
        }
        return Ok(list);
    }


    [APIStaticAuth]
    // Post LATEST weather data for a specific location
    [HttpPost("{location}", Name = "StoreNewData")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<WeatherDTOModel> StoreNewData([FromBody] WeatherDTOModel dataModel, string location)
    {
        location = location.ToLower();
        var _location = _dbcontext.Location
                    .Where(u => u.State == location)
                    .FirstOrDefault();
        if (_location is null)
        {
            _logger.LogWarning("____Attempted invalid location!");
            return BadRequest($"{location} is invalid location!");
        }
        if (dataModel is null) return BadRequest(dataModel);
        WeatherDataModel model = _mapper.Map<WeatherDataModel>(dataModel);
        model.Location = _location;
        _dbcontext.Weather.Add(model);
        _dbcontext.SaveChanges();
        _logger.LogInformation($"____New entry at {location}.");
        return CreatedAtRoute("GetCurrentData", new { location = location }, dataModel);
    }


    [APIStaticAuth]
    [HttpDelete("{location}", Name = "DeleteLastEntry")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteWeatherData(string location)
    {
        location = location.ToLower();
        var _location = _dbcontext.Location
                    .Where(u => u.State == location)
                    .FirstOrDefault();
        if (_location is null)
        {
            _logger.LogWarning("____Attempted invalid location!");
            return NotFound($"{location} is invalid location!");
        }
        if (_dbcontext.Weather.ToList().Count == 0)
        {
            _logger.LogError("____Data repository currently empty!");
            return NoContent();
        }
        var reference = _dbcontext.Weather
                .Where(u => u.Location == new LocationModel
                {
                    State = location
                }).OrderBy(u => u.Time).LastOrDefault();
        if (reference is not null) 
        {
            _dbcontext.Weather.Remove(reference);
            _logger.LogWarning($"____Last entry from {location} was deleted!");
        }
        _dbcontext.SaveChanges();
        return NoContent();
    }

    // <<API endpoints>>
}


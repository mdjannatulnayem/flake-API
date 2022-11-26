using AutoMapper;
using flake_API.Models;
using flake_API.Models.dtoModels;

namespace flake_API;

public class MappingConfig : Profile
{
	public MappingConfig()
	{
		CreateMap<WeatherDataModel,WeatherDTOModel>()
			.ReverseMap();
	}
}

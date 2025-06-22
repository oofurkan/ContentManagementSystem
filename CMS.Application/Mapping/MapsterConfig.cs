using CMS.Domain.Entities;
using CMS.Application.DTOs;
using Mapster;

namespace CMS.Application.Mapping
{
	public static class MapsterConfig
	{
		public static void RegisterMappings()
		{
			TypeAdapterConfig<User, UserDto>.NewConfig();
			TypeAdapterConfig<Content, ContentDto>.NewConfig()
				.Map(dest => dest.CategoryName, src => src.Category.Name);
		}
	}
}

using CMS.Application.DTOs;
using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using CMS.Domain.Interfaces;
using Mapster;

namespace CMS.Infrastructure.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<UserDto> CreateUserAsync(UserDto dto)
		{
			var user = dto.Adapt<User>(); // Mapster dönüşümü
			user.Id = Guid.NewGuid();

			await _userRepository.AddAsync(user);

			return user.Adapt<UserDto>();
		}

		public async Task<IEnumerable<ContentDto>> GetUserContentsAsync(Guid userId)
		{
			var user = await _userRepository.GetByIdAsync(userId);

			if (user == null || user.Contents == null)
				return Enumerable.Empty<ContentDto>();

			// Her içerik için category bilgisi DTO’ya yazılıyor
			var result = user.Contents.Select(c =>
			{
				var dto = c.Adapt<ContentDto>();
				dto.CategoryName = c.Category?.Name;
				return dto;
			});

			return result;
		}
	}
}

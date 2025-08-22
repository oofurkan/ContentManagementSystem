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
			// Check if user with same email already exists
			var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
			if (existingUser != null)
			{
				throw new InvalidOperationException($"User with email '{dto.Email}' already exists.");
			}

			var user = dto.Adapt<User>(); // Mapster dönüşümü
			user.Id = Guid.NewGuid();

			await _userRepository.AddAsync(user);

			return user.Adapt<UserDto>();
		}

		public async Task<UserDto> CreateUserWithAuthAsync(UserCreateDto dto)
		{
			// Check if user with same email already exists
			var existingUserByEmail = await _userRepository.GetByEmailAsync(dto.Email);
			if (existingUserByEmail != null)
			{
				throw new InvalidOperationException($"User with email '{dto.Email}' already exists.");
			}

			// Check if user with same username already exists
			var existingUserByUsername = await _userRepository.GetByUsernameAsync(dto.Username);
			if (existingUserByUsername != null)
			{
				throw new InvalidOperationException($"User with username '{dto.Username}' already exists.");
			}

			var user = new User
			{
				Id = Guid.NewGuid(),
				FullName = dto.FullName,
				Email = dto.Email,
				Username = dto.Username,
				PasswordHash = AuthService.HashPassword(dto.Password)
			};

			await _userRepository.AddAsync(user);

			return new UserDto
			{
				FullName = user.FullName,
				Email = user.Email
			};
		}

		public async Task<IEnumerable<ContentDto>> GetUserContentsAsync(Guid userId)
		{
			var user = await _userRepository.GetByIdAsync(userId);

			if (user == null || user.Contents == null)
				return Enumerable.Empty<ContentDto>();

			// Mapster now handles CategoryName mapping automatically
			return user.Contents.Select(c => c.Adapt<ContentDto>());
		}

		public async Task<User?> GetUserByUsernameAsync(string username)
		{
			return await _userRepository.GetByUsernameAsync(username);
		}

		public async Task<bool> ValidateCredentialsAsync(string username, string password)
		{
			var user = await _userRepository.GetByUsernameAsync(username);
			if (user == null)
				return false;

			return AuthService.VerifyPassword(password, user.PasswordHash);
		}

	}
}

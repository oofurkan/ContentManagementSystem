using CMS.Application.DTOs;
using CMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace CMS.WebAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost]
		public async Task<IActionResult> CreateUser([FromBody] UserDto dto)
		{
			var createdUser = await _userService.CreateUserAsync(dto);
			return Ok(createdUser);
		}

		[HttpGet("{userId}/contents")]
		public async Task<IActionResult> GetUserContents(Guid userId)
		{
			var contents = await _userService.GetUserContentsAsync(userId);
			return Ok(contents);
		}
	}
}

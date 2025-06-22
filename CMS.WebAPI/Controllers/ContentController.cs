using CMS.Application.DTOs;
using CMS.Application.Interfaces;
using CMS.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace CMS.WebAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ContentController : ControllerBase
	{
		private readonly IContentService _contentService;

		public ContentController(IContentService contentService)
		{
			_contentService = contentService;
		}

		// Kullanıcıya özel varyantlı içerik
		[HttpGet("{contentId}/user/{userId}")]
		public async Task<IActionResult> GetContentForUser(Guid userId, Guid contentId)
		{
			var content = await _contentService.GetContentForUserAsync(userId, contentId);
			if (content == null)
				return NotFound();

			return Ok(content);
		}

		// Filtreleme (kategori ve dil ile)
		[HttpGet("filter")]
		public async Task<IActionResult> FilterContents([FromQuery] string? language, [FromQuery] string? category)
		{
			var filtered = await _contentService.FilterContentsAsync(language, category);
			return Ok(filtered);
		}

		[HttpPost]
		public async Task<IActionResult> CreateContent([FromBody] ContentCreateDto dto)
		{
			await _contentService.AddContentAsync(dto);
			return Ok("Content created successfully");
		}

		[HttpPost("category")]
		public async Task<IActionResult> CreateCategory([FromBody] CategoryDto dto)
		{
			await _contentService.AddCategoryAsync(dto);
			return Ok("Category created successfully");
		}


	}
}

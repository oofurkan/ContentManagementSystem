using CMS.Application.DTOs;
using CMS.Application.Interfaces;
using CMS.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
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
	[Authorize]
	public async Task<IActionResult> FilterContents([FromQuery] string? language, [FromQuery] string? category)
	{
		var filtered = await _contentService.FilterContentsAsync(language, category);
		return Ok(filtered);
	}

	[HttpPost]
	[Authorize]
	public async Task<IActionResult> CreateContent([FromBody] ContentCreateDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				await _contentService.AddContentAsync(dto);
				return Ok("Content created successfully");
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

	[HttpPost("category")]
	[Authorize]
	public async Task<IActionResult> CreateCategory([FromBody] CategoryDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				await _contentService.AddCategoryAsync(dto);
				return Ok("Category created successfully");
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

	// Kategori listesini getir
	[HttpGet("categories")]
	public async Task<IActionResult> GetCategories()
	{
		var categories = await _contentService.GetCategoriesAsync();
		return Ok(categories);
	}

	}
}

	

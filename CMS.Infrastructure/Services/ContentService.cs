using CMS.Application.DTOs;
using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using CMS.Domain.Interfaces;
using CMS.Infrastructure.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CMS.Infrastructure.Services
{
	public class ContentService : IContentService
	{
		private readonly IContentRepository _contentRepository;
		private readonly IMemoryCache _memoryCache;

		private readonly AppDbContext _context;


		public ContentService(AppDbContext context, IContentRepository contentRepository, IMemoryCache memoryCache)
		{
			_context = context;
			_contentRepository = contentRepository;
			_memoryCache = memoryCache;
		}

		public async Task<ContentDto> GetContentForUserAsync(Guid userId, Guid contentId)
		{
			var content = await _contentRepository.GetByIdAsync(contentId);

			if (content == null || content.Variants == null || !content.Variants.Any())
				return null;

			// 1️⃣ Varyant cache kontrolü
			var cacheKey = $"content_variant:{userId}:{contentId}";
			if (!_memoryCache.TryGetValue(cacheKey, out ContentVariant selectedVariant))
			{
				// 2️⃣ Rastgele bir varyant seç
				selectedVariant = content.Variants.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();

				// 3️⃣ Cache'e kaydet (15 dakika süreyle)
				_memoryCache.Set(cacheKey, selectedVariant, TimeSpan.FromMinutes(15));
			}

			// 4️⃣ DTO oluştur ve varyant metnini içine ekle
			var dto = content.Adapt<ContentDto>();
			dto.CategoryName = content.Category?.Name;
			dto.Description = selectedVariant?.VariantText; // varyant içeriğini göster

			return dto;
		}

		public async Task<IEnumerable<ContentDto>> FilterContentsAsync(string language, string category)
		{
			// Not: Daha optimize versiyonu için repository’ye özel filtre metodu yazılabilir
			var allContents = await _contentRepository.GetByUserIdAsync(Guid.Empty); // tüm içerikleri çekmek için geçici kullanım

			var filtered = allContents
				.Where(c =>
					(string.IsNullOrEmpty(language) || c.Language == language) &&
					(string.IsNullOrEmpty(category) || c.Category?.Name == category))
				.Select(c =>
				{
					var dto = c.Adapt<ContentDto>();
					dto.CategoryName = c.Category?.Name;
					return dto;
				});

			return filtered;
		}

		public async Task AddContentAsync(ContentCreateDto dto)
		{
			var content = new Content
			{
				Id = Guid.NewGuid(),
				Title = dto.Title,
				Description = dto.Description,
				Language = dto.Language,
				ImageUrl = dto.ImageUrl,
				CategoryId = dto.CategoryId,
				UserId = dto.UserId,
				Variants = dto.Variants.Select(v => new ContentVariant
				{
					Id = Guid.NewGuid(),
					VariantText = v
				}).ToList()
			};

			_context.Contents.Add(content);
			await _context.SaveChangesAsync();
		}

		public async Task AddCategoryAsync(CategoryDto dto)
		{
			var category = new Category
			{
				Id = Guid.NewGuid(),
				Name = dto.Name
			};

			_context.Categories.Add(category);
			await _context.SaveChangesAsync();
		}

	}
}

using CMS.Application.DTOs;
using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using CMS.Domain.Interfaces;
using Mapster;
using Microsoft.Extensions.Caching.Memory;

namespace CMS.Infrastructure.Services
{
	public class ContentService : IContentService
	{
		private readonly IContentRepository _contentRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMemoryCache _memoryCache;

		public ContentService(IContentRepository contentRepository, ICategoryRepository categoryRepository, IMemoryCache memoryCache)
		{
			_contentRepository = contentRepository;
			_categoryRepository = categoryRepository;
			_memoryCache = memoryCache;
		}

		public async Task<ContentDto?> GetContentForUserAsync(Guid userId, Guid contentId)
		{
			var content = await _contentRepository.GetByIdAsync(contentId);

			if (content == null || content.Variants == null || !content.Variants.Any())
				return null;

			// 1️⃣ Varyant cache kontrolü
			var cacheKey = $"content_variant:{userId}:{contentId}";
			if (!_memoryCache.TryGetValue(cacheKey, out ContentVariant? selectedVariant))
			{
				// 2️⃣ Rastgele bir varyant seç
				selectedVariant = content.Variants.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();

				// 3️⃣ Cache'e kaydet (15 dakika süreyle)
				_memoryCache.Set(cacheKey, selectedVariant, TimeSpan.FromMinutes(15));
			}

			// 4️⃣ DTO oluştur ve varyant metnini içine ekle
			var dto = content.Adapt<ContentDto>();
			dto.Description = selectedVariant?.VariantText ?? dto.Description; // varyant içeriğini göster

			return dto;
		}

		public async Task<IEnumerable<ContentDto>> FilterContentsAsync(string? language, string? category)
		{
			// Fixed: Use the new repository method for proper filtering
			var filteredContents = await _contentRepository.FilterAsync(language, category);

			return filteredContents.Select(c => c.Adapt<ContentDto>());
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

			await _contentRepository.AddAsync(content);
		}

		public async Task AddCategoryAsync(CategoryDto dto)
		{
			// Check if category with same name already exists
			var existingCategory = await _categoryRepository.GetByNameAsync(dto.Name);
			if (existingCategory != null)
			{
				throw new InvalidOperationException($"Category with name '{dto.Name}' already exists.");
			}

			var category = new Category
			{
				Id = Guid.NewGuid(),
				Name = dto.Name
			};

			await _categoryRepository.AddAsync(category);
		}
	}
}

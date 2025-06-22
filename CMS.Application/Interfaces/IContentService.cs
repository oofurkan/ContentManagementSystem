using CMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CMS.Application.DTOs;

namespace CMS.Application.Interfaces
{
	public interface IContentService
	{
		Task<ContentDto> GetContentForUserAsync(Guid userId, Guid contentId);
		Task<IEnumerable<ContentDto>> FilterContentsAsync(string language, string category);
		Task AddContentAsync(ContentCreateDto dto);
		Task AddCategoryAsync(CategoryDto dto);

	}
}


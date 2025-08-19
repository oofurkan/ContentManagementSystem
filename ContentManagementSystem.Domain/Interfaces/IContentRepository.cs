using CMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Interfaces
{
	public interface IContentRepository
	{
		Task<Content?> GetByIdAsync(Guid id);
		Task<IEnumerable<Content>> GetByUserIdAsync(Guid userId);
		Task<IEnumerable<Content>> GetAllAsync();
		Task<IEnumerable<Content>> FilterAsync(string? language, string? categoryName);
		Task AddAsync(Content content);
	}
}

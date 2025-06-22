using CMS.Domain.Entities;
using CMS.Domain.Interfaces;
using CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CMS.Infrastructure.Repositories
{
	public class ContentRepository : IContentRepository
	{
		private readonly AppDbContext _context;

		public ContentRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Content> GetByIdAsync(Guid id)
		{
			return await _context.Contents
				.Include(c => c.Variants)
				.Include(c => c.Category)
				.Include(c => c.User)
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<IEnumerable<Content>> GetByUserIdAsync(Guid userId)
		{
			return await _context.Contents
				.Where(c => c.UserId == userId)
				.Include(c => c.Variants)
				.Include(c => c.Category)
				.ToListAsync();
		}
	}
}

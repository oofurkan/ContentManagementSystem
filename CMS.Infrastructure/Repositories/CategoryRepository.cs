using CMS.Domain.Entities;
using CMS.Domain.Interfaces;
using CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CMS.Infrastructure.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly AppDbContext _context;

		public CategoryRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Category?> GetByIdAsync(Guid id)
		{
			return await _context.Categories
				.Include(c => c.Contents)
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<Category?> GetByNameAsync(string name)
		{
			return await _context.Categories
				.FirstOrDefaultAsync(c => c.Name == name);
		}

		public async Task<IEnumerable<Category>> GetAllAsync()
		{
			return await _context.Categories
				.Include(c => c.Contents)
				.ToListAsync();
		}

		public async Task AddAsync(Category category)
		{
			await _context.Categories.AddAsync(category);
			await _context.SaveChangesAsync();
		}
	}
}



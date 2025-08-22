using CMS.Domain.Entities;
using CMS.Domain.Interfaces;
using CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CMS.Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext _context;

		public UserRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task AddAsync(User user)
		{
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
		}

		public async Task<User?> GetByIdAsync(Guid id)
		{
			return await _context.Users
				.Include(u => u.Contents)
					.ThenInclude(c => c.Variants)
				.Include(u => u.Contents)
					.ThenInclude(c => c.Category)
				.FirstOrDefaultAsync(u => u.Id == id);
		}

		public async Task<User?> GetByEmailAsync(string email)
		{
			return await _context.Users
				.FirstOrDefaultAsync(u => u.Email == email);
		}

		public async Task<User?> GetByUsernameAsync(string username)
		{
			return await _context.Users
				.FirstOrDefaultAsync(u => u.Username == username);
		}
	}
}

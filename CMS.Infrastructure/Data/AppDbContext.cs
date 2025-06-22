using Microsoft.EntityFrameworkCore;
using CMS.Domain.Entities;

namespace CMS.Infrastructure.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Content> Contents { get; set; }
		public DbSet<ContentVariant> ContentVariants { get; set; }
		public DbSet<Category> Categories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// User -> Content (One to Many)
			modelBuilder.Entity<User>()
				.HasMany(u => u.Contents)
				.WithOne(c => c.User)
				.HasForeignKey(c => c.UserId);

			// Content -> Category (Many to One)
			modelBuilder.Entity<Content>()
				.HasOne(c => c.Category)
				.WithMany(cat => cat.Contents)
				.HasForeignKey(c => c.CategoryId);

			// Content -> ContentVariant (One to Many)
			modelBuilder.Entity<Content>()
				.HasMany(c => c.Variants)
				.WithOne(v => v.Content)
				.HasForeignKey(v => v.ContentId);
		}
	}
}

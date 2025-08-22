using CMS.Domain.Entities;
using CMS.Infrastructure.Services;

namespace CMS.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task SeedDataAsync(AppDbContext context)
        {
            await SeedCategoriesAsync(context);
            await SeedUsersAsync(context);
        }

        public static async Task SeedCategoriesAsync(AppDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Id = Guid.NewGuid(), Name = "Technology" },
                    new Category { Id = Guid.NewGuid(), Name = "Business" },
                    new Category { Id = Guid.NewGuid(), Name = "Health" },
                    new Category { Id = Guid.NewGuid(), Name = "Education" },
                    new Category { Id = Guid.NewGuid(), Name = "Entertainment" }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedUsersAsync(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                var defaultUser = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = "Admin User",
                    Email = "admin@cms.com",
                    Username = "admin",
                    PasswordHash = AuthService.HashPassword("admin123")
                };

                await context.Users.AddAsync(defaultUser);
                await context.SaveChangesAsync();
            }
        }
    }
}

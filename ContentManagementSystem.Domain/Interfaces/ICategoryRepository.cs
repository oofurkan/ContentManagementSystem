using CMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Interfaces
{
	public interface ICategoryRepository
	{
		Task<Category?> GetByIdAsync(Guid id);
		Task<Category?> GetByNameAsync(string name);
		Task<IEnumerable<Category>> GetAllAsync();
		Task AddAsync(Category category);
	}
}



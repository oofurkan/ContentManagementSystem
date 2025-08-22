using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities
{
	public sealed class User
	{
		public Guid Id { get; set; }
		public string FullName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Username { get; set; } = string.Empty;
		public string PasswordHash { get; set; } = string.Empty;
		public ICollection<Content> Contents { get; set; } = new List<Content>();
	}
}

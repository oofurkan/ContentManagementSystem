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
		public string FullName { get; set; }
		public string Email { get; set; }
		public ICollection<Content> Contents { get; set; }
	}
}

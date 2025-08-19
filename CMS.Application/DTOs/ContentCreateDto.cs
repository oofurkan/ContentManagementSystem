using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.DTOs
{
	public class ContentCreateDto
	{
		[Required]
		[StringLength(200)]
		public string Title { get; set; } = string.Empty;
		
		[Required]
		public string Description { get; set; } = string.Empty;
		
		[Required]
		[StringLength(10)]
		public string Language { get; set; } = string.Empty;
		
		[Required]
		[Url]
		public string ImageUrl { get; set; } = string.Empty;
		
		[Required]
		public Guid CategoryId { get; set; }
		
		[Required]
		public Guid UserId { get; set; }
		
		[Required]
		[MinLength(1)]
		public List<string> Variants { get; set; } = new List<string>();
	}
}


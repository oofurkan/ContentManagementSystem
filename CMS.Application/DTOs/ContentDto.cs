using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.DTOs
{
    public class ContentDto
    {
        public Guid Id { get; set; }
        
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
        
        public string? CategoryName { get; set; }
    }
}

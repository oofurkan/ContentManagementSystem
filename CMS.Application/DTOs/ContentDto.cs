using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.DTOs
{
    public class ContentDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
    }
}

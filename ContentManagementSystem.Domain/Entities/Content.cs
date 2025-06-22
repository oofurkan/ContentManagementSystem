using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities
{
    public sealed class Content
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string ImageUrl { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<ContentVariant> Variants { get; set; }
    }
}

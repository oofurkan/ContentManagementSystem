using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Domain.Entities
{
    public sealed class ContentVariant
    {
        public Guid Id { get; set; }
        public string VariantText { get; set; }
        public Guid ContentId { get; set; }
        public Content Content { get; set; }
    }
}

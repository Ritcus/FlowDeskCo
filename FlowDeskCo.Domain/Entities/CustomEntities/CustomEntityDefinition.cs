using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestateCo.Domain.Entities.CustomEntities
{
    public class CustomEntityDefinition
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string? Name { get; set; } // e.g. "Property"
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<CustomEntityFieldDefinition>? Fields { get; set; }
    }
}

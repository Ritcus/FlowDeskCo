using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestateCo.Domain.Entities.CustomEntities
{
    public class CustomEntityFieldDefinition
    {
        public Guid Id { get; set; }
        public Guid CustomEntityDefinitionId { get; set; }
        public string? Name { get; set; } // e.g. "Address"
        public string? DataType { get; set; } // e.g. "string", "int", "bool", "datetime"
        public bool IsRequired { get; set; }
        public int? MaxLength { get; set; }
    }
}

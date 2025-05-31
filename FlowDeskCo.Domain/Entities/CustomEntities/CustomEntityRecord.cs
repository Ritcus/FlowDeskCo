using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestateCo.Domain.Entities.CustomEntities
{
    public class CustomEntityRecord
    {
        public Guid Id { get; set; }
        public Guid CustomEntityDefinitionId { get; set; }
        public Guid ClientId { get; set; }
        public string? DataJson { get; set; } // store entity record data as JSON
        public DateTime CreatedAt { get; set; }
    }
}

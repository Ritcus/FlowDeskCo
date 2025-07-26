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
        public string? DataJson { get; set; }
        public Guid? CreatedByUserId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

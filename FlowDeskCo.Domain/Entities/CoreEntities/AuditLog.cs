using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestateCo.Domain.Entities.CoreEntities
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public string? Action { get; set; }
        public Guid? ClientId { get; set; }
        public string? PerformedBy { get; set; }
        public string? EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string? Details { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

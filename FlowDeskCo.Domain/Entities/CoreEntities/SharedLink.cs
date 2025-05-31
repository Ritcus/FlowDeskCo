using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestateCo.Domain.Entities.CoreEntities
{
    public class SharedLink
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public string? ShareCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}

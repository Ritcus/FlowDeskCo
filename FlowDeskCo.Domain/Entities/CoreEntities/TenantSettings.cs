using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDeskCo.Domain.Entities.CoreEntities
{
    public class TenantSettings
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; } // links to your Client entity
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}

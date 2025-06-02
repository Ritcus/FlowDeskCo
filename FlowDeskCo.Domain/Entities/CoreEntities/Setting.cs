using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestateCo.Domain.Entities.CoreEntities
{
    public class Setting
    {
        public Guid Id { get; set; }
        public Guid? ClientId { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}

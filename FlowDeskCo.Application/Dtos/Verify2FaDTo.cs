using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDeskCo.Application.Dtos
{
    public class Verify2FaDto
    {
        public required string Email { get; set; }
        public required string Code { get; set; }
        public string Purpose { get; set; }
        public bool RememberMe { get; set; }

    }
}

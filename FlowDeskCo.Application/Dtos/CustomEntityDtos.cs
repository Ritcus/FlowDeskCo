using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlowDeskCo.Application.Dtos
{
    public class CreateCustomEntityDefinitionDto
    {
        public Guid? ClientId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<CreateCustomEntityFieldDefinitionDto> Fields { get; set; } = new();
    }
    public class CreateCustomEntityFieldDefinitionDto
    {
        public string Name { get; set; } = null!;
        public string DataType { get; set; } = null!;
        public bool IsRequired { get; set; }
        public int? MaxLength { get; set; }
    }

    public class CreateCustomEntityRecordDto
    {
        public Guid CustomEntityDefinitionId { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? CreatedByUserId { get; set; }
        public JsonElement Data { get; set; }
    }
}

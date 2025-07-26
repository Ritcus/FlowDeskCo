using FlowDeskCo.Application.Dtos;
using RestateCo.Domain.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlowDeskCo.Application.Interfaces
{
    public interface ICustomEntityService
    {
        Task<CustomEntityDefinition> CreateEntityDefinitionAsync(CreateCustomEntityDefinitionDto dto);
        Task<IEnumerable<CustomEntityDefinition>> GetAllEntityDefinitionsAsync();
        Task<CustomEntityDefinition?> GetEntityDefinitionByIdAsync(Guid id);

        Task<CustomEntityRecord> CreateRecordAsync(CreateCustomEntityRecordDto dto);
        Task<CustomEntityRecord?> GetRecordByIdAsync(Guid id);
        Task<IEnumerable<CustomEntityRecord>> GetRecordsAsync(Guid? entityDefinitionId, Guid? userId);
        Task UpdateRecordAsync(Guid id, JsonElement dataJson);
        Task DeleteRecordAsync(Guid id);

        Task<bool> ValidateRecordAsync(Guid entityDefinitionId, JsonElement data);
    }
}

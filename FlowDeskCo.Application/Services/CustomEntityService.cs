using FlowDeskCo.Application.Dtos;
using FlowDeskCo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using RestateCo.Domain.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlowDeskCo.Application.Services
{
    [Authorize]
    public class CustomEntityService : ICustomEntityService
    {
        private readonly ICustomEntityRepository _entityDefRepo;
        private readonly IGenericRepository<CustomEntityRecord> _recordRepo;
        private readonly Guid _clientId;

        public CustomEntityService(
            ICustomEntityRepository entityDefRepo,
            IGenericRepository<CustomEntityRecord> recordRepo, ITenantProvider tenantProvider)
        {
            _entityDefRepo = entityDefRepo;
            _recordRepo = recordRepo;
            _clientId = tenantProvider.GetTenantId();
        }

       

        public async Task<CustomEntityDefinition> CreateEntityDefinitionAsync(CreateCustomEntityDefinitionDto dto)
        {
            var entityDef = new CustomEntityDefinition
            {
                Id = Guid.NewGuid(),
                ClientId = _clientId,
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                Fields = dto.Fields.Select(f => new CustomEntityFieldDefinition
                {
                    Id = Guid.NewGuid(),
                    Name = f.Name,
                    DataType = f.DataType,
                    IsRequired = f.IsRequired,
                    MaxLength = f.MaxLength
                }).ToList()
            };

            await _entityDefRepo.AddAsync(entityDef);
            return entityDef;
        }

        public async Task<IEnumerable<CustomEntityDefinition>> GetAllEntityDefinitionsAsync()
        {
            var allDefs = await _entityDefRepo.GetAllWithFieldsAsync(d => d.ClientId == _clientId);
            return allDefs;
        }

        public async Task<CustomEntityDefinition?> GetEntityDefinitionByIdAsync(Guid id)
        {
            return await _entityDefRepo.GetWithFieldsByIdAsync(id);
        }

        
        public async Task<CustomEntityRecord> CreateRecordAsync(CreateCustomEntityRecordDto dto)
        {
            // Validate first
            var isValid = await ValidateRecordAsync(dto.CustomEntityDefinitionId, dto.Data);

            if (!isValid)
                throw new ArgumentException("Record data is invalid.");

            var record = new CustomEntityRecord
            {
                Id = Guid.NewGuid(),
                ClientId = _clientId,
                CustomEntityDefinitionId = dto.CustomEntityDefinitionId,
                DataJson = JsonSerializer.Serialize(dto.Data),
                CreatedAt = DateTime.UtcNow
            };

            await _recordRepo.AddAsync(record);
            return record;
        }

        public async Task<CustomEntityRecord?> GetRecordByIdAsync(Guid id)
        {
            return await _recordRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<CustomEntityRecord>> GetRecordsAsync(Guid? entityDefinitionId, Guid? userId)
        {
            if(entityDefinitionId != null)
                return await _recordRepo.GetAllAsync(a => a.CreatedByUserId == userId && a.ClientId == _clientId);
            
            if(userId != null)
                return await _recordRepo.GetAllAsync(a => a.ClientId == _clientId && a.CustomEntityDefinitionId == entityDefinitionId);

            return await _recordRepo.GetAllAsync(a => a.ClientId == _clientId);
        }

        public async Task UpdateRecordAsync(Guid id, JsonElement dataJson)
        {
            var record = await _recordRepo.GetByIdAsync(id);
            if (record == null) throw new KeyNotFoundException("Record not found.");

            var isValid = await ValidateRecordAsync(record.CustomEntityDefinitionId, dataJson);
            if (!isValid)
                throw new ArgumentException("Record data is invalid.");

            record.DataJson = JsonSerializer.Serialize(dataJson);
            await _recordRepo.UpdateAsync(record);
        }

        public async Task DeleteRecordAsync(Guid id)
        {
            var record = await _recordRepo.GetByIdAsync(id);
            if (record == null) throw new KeyNotFoundException("Record not found.");
            await _recordRepo.DeleteAsync(record);
        }

        // Basic validation example
        public async Task<bool> ValidateRecordAsync(Guid entityDefinitionId, JsonElement data)
        {
            var entityDef = await _entityDefRepo.GetWithFieldsByIdAsync(entityDefinitionId);
            if (entityDef == null || entityDef.Fields == null) return false;

            // Deserialize the data JSON
            Dictionary<string, JsonElement>? dataDic;
            try
            {
                dataDic = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(data);
            }
            catch
            {
                return false;
            }
            if (dataDic == null) return false;

            foreach (var field in entityDef.Fields)
            {
                if (!dataDic.ContainsKey(field.Name!))
                {
                    if (field.IsRequired)
                        return false; // missing required field
                    else
                        continue;
                }

                var value = dataDic[field.Name!];

                // Example: simple type checks (expand as needed)
                switch (field.DataType?.ToLower())
                {
                    case "string":
                        if (value.ValueKind != JsonValueKind.String) return false;
                        if (field.MaxLength.HasValue && value.GetString()!.Length > field.MaxLength.Value) return false;
                        break;

                    case "int":
                    case "integer":
                        if (value.ValueKind != JsonValueKind.Number || !value.TryGetInt32(out _)) return false;
                        break;

                    case "bool":
                    case "boolean":
                        if (value.ValueKind != JsonValueKind.True && value.ValueKind != JsonValueKind.False) return false;
                        break;

                    case "datetime":
                        if (value.ValueKind != JsonValueKind.String ||
                            !DateTime.TryParse(value.GetString(), out _)) return false;
                        break;

                    default:
                        // unknown types, consider invalid for now
                        return false;
                }
            }

            return true;
        }
    }
}

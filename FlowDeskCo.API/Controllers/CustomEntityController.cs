using FlowDeskCo.Application.Dtos;
using FlowDeskCo.Application.Extensions;
using FlowDeskCo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace FlowDeskCo.API.Controllers
{
    [ApiController]
    [Route("api/custom-entities")]
    [Authorize]
    public class CustomEntityController : ControllerBase
    {
        private readonly ICustomEntityService _customEntityService;

        public CustomEntityController(ICustomEntityService customEntityService)
        {
            _customEntityService = customEntityService;
        }
        [Authorize]
        [HttpPost("definitions")]
        public async Task<IActionResult> CreateDefinition([FromBody] CreateCustomEntityDefinitionDto dto)
        {
            var claimIdClaim = User.GetUserId().ToString();
            if (string.IsNullOrEmpty(claimIdClaim))
                return Unauthorized("Invalid or missing client_id claim");

            var entityDef = await _customEntityService.CreateEntityDefinitionAsync(dto);
            return CreatedAtAction(nameof(GetDefinitionById), new { id = entityDef.Id }, entityDef);
        }

        [HttpGet("definitions/{id}")]
        public async Task<IActionResult> GetDefinitionById(Guid id)
        {
            var entityDef = await _customEntityService.GetEntityDefinitionByIdAsync(id);
            if (entityDef == null) return NotFound();
            return Ok(entityDef);
        }

        [HttpGet("definitions")]
        public async Task<IActionResult> GetDefinitions()
        {
            var defs = await _customEntityService.GetAllEntityDefinitionsAsync();
            return Ok(defs);
        }

        [HttpPost("records")]
        public async Task<IActionResult> CreateRecord([FromBody] CreateCustomEntityRecordDto dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
                dto.CreatedByUserId =Guid.Parse(userId);

            if (dto.Data.ValueKind != JsonValueKind.Object)
                return BadRequest("Invalid JSON payload");

            try
            {
                var record = await _customEntityService.CreateRecordAsync(dto);
                return CreatedAtAction(nameof(GetRecordById), new { id = record.Id }, record);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("records/{id}")]
        public async Task<IActionResult> GetRecordById(Guid id)
        {
            var record = await _customEntityService.GetRecordByIdAsync(id);
            if (record == null) return NotFound();
            return Ok(record);
        }

        [HttpGet("records")]
        public async Task<IActionResult> GetRecordsByUser()
        {
            var userId = User.GetUserId();
            if(userId == null)
                return Unauthorized("User ID not found in claims.");

            var records = await _customEntityService.GetRecordsAsync(null, userId);
            return Ok(records);
        }

        [HttpGet("entity/records")]
        public async Task<IActionResult> GetRecordsByEntity([FromQuery] Guid entityId)
        {
            var records = await _customEntityService.GetRecordsAsync(entityId, null);
            return Ok(records);
        }

        [HttpPut("records/{id}")]
        public async Task<IActionResult> UpdateRecord(Guid id, [FromBody] JsonElement dataJson)
        {
            try
            {
                await _customEntityService.UpdateRecordAsync(id, dataJson);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("records/{id}")]
        public async Task<IActionResult> DeleteRecord(Guid id)
        {
            try
            {
                await _customEntityService.DeleteRecordAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}

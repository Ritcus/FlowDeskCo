using FlowDeskCo.Application.Interfaces;
using FlowDeskCo.Domain.Entities.CoreEntities;
using FlowDeskCo.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDeskCo.Application.Services
{
    public class TenantSettingsService : ITenantSettingsService
    {
        private readonly IGenericRepository<TenantSettings> _repository;
        private readonly Guid _clientId;

        public TenantSettingsService (IGenericRepository<TenantSettings> repository, ITenantProvider tenantProvider)
        {
            _repository = repository;
            _clientId = tenantProvider.GetTenantId();
        }

        public async Task<string?> GetSettingAsync( string key)
        {
            var allsettings = await _repository.GetAllAsync(s => s.ClientId == _clientId);
            return allsettings.Where(a  => a.Key == key).FirstOrDefault()?.Value;
        }

        public async Task<IEnumerable<TenantSettings>> GetSettingsForClientAsync()
        {
            return await _repository.GetAllAsync(s => s.ClientId == _clientId);
        }

        public async Task AddOrUpdateSettingAsync(string key, string value)
        {
            var allsettings = await _repository.GetAllAsync(s => s.ClientId == _clientId);

            var setting = allsettings.Where(a => a.Key == key).FirstOrDefault();

            if (setting == null)
            {
                setting = new TenantSettings
                {
                    Id = Guid.NewGuid(),
                    ClientId = _clientId,
                    Key = key,
                    Value = value
                };
                await _repository.AddAsync(setting);
            }
            else
            {
                setting.Value = value;
                await _repository.UpdateAsync(setting);
            }
        }

    }
}

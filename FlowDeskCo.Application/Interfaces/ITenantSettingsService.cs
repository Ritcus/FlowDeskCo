using FlowDeskCo.Domain.Entities.CoreEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDeskCo.Application.Interfaces
{
    public interface ITenantSettingsService
    {
        Task<string?> GetSettingAsync(string key);
        Task<IEnumerable<TenantSettings>> GetSettingsForClientAsync();
        Task AddOrUpdateSettingAsync( string key, string value);
    }
}

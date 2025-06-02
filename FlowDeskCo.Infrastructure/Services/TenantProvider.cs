using FlowDeskCo.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using RestateCo.Domain.Entities.CoreEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDeskCo.Infrastructure.Services
{
    public class TenantProvider :ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetTenantId()
        {
            var clientIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("client_id")?.Value;
            return string.IsNullOrEmpty(clientIdClaim) ? Guid.Empty : Guid.Parse(clientIdClaim);
        }
    }
}

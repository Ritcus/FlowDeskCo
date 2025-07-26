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
            if(_httpContextAccessor.HttpContext?.Items["ClientId"] is Guid clientId)
        {
                return clientId;
            }

            return Guid.Parse("33233333-3333-3333-3333-333333333333");
        }
    }
}

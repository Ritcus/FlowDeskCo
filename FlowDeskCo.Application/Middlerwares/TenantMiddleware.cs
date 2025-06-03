using FlowDeskCo.Application.Interfaces;
using FlowDeskCo.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDeskCo.Application.Middlerwares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IClientRepository clientRepo)
        {
            var host = context.Request.Host.Host; // something.myapp.com.au
            var subdomain = host.Split('.').FirstOrDefault();

            if (!string.IsNullOrEmpty(subdomain) && subdomain != "localhost")
            {
                var client = await clientRepo.GetBySlugAsync(subdomain);
                if (client != null)
                {
                    context.Items["ClientId"] = client.Id;
                }
                else
                {
                    // Optionally set a default or reject request if invalid tenant
                    context.Items["ClientId"] = Guid.Empty;
                }
            }
            else
            {
                // In localhost or fallback scenario
                context.Items["ClientId"] = Guid.Parse("32233333-3333-3333-3333-333333333333");
            }

            await _next(context);
        }
    }

}

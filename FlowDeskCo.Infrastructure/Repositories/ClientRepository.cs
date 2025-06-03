using FlowDeskCo.Application.Interfaces;
using FlowDeskCo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RestateCo.Domain.Entities.CoreEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDeskCo.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Client?> GetBySlugAsync(string slug)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Slug == slug);
        }
    }

}

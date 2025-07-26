using FlowDeskCo.Application.Interfaces;
using FlowDeskCo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RestateCo.Domain.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FlowDeskCo.Infrastructure.Repositories
{
    public class CustomEntityRepository : GenericRepository<CustomEntityDefinition>, ICustomEntityRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<CustomEntityDefinition> _dbSet;

        public CustomEntityRepository (AppDbContext appDbContext): base (appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet =_appDbContext.Set<CustomEntityDefinition>();
        }

        public async Task<CustomEntityDefinition?> GetWithFieldsByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(d => d.Fields)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<CustomEntityDefinition>> GetAllWithFieldsAsync(Expression<Func<CustomEntityDefinition, bool>>? predicate = null)
        {
            IQueryable<CustomEntityDefinition> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);


            return await query.Include(d => d.Fields).ToListAsync();
        }
    }
}

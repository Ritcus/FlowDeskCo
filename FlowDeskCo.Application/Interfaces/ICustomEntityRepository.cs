using RestateCo.Domain.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FlowDeskCo.Application.Interfaces
{
    public interface ICustomEntityRepository : IGenericRepository<CustomEntityDefinition>
    {
        Task<IEnumerable<CustomEntityDefinition>> GetAllWithFieldsAsync(Expression<Func<CustomEntityDefinition, bool>>? predicate = null);
        Task<CustomEntityDefinition?> GetWithFieldsByIdAsync(Guid id);

    }
}

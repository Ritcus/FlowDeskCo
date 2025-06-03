using RestateCo.Domain.Entities.CoreEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowDeskCo.Application.Interfaces
{
    public interface IClientRepository
    {
        Task<Client?> GetBySlugAsync(string slug);
    }
}

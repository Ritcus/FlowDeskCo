using Microsoft.AspNetCore.Identity;

namespace RestateCo.Domain.Entities.CoreEntities
{
    public class Role : IdentityRole<Guid>
    {
        public string? Description { get; set; }
    }
}

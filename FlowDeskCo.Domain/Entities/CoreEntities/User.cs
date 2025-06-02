using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RestateCo.Domain.Entities.CoreEntities
{
    public enum UserType
    {
        Employee,
        Client
    }


    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; } = null!;
        public Guid? ClientId { get; set; }
        public Client? Client { get; set; }
        public Guid RoleId { get; set; }
        public Role? Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }


}

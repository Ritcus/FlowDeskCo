using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestateCo.Domain.Entities.CoreEntities
{
    public class VerificationCode
    {
        public Guid Id { get; set; }
        public string? Recipient { get; set; } // Email or PhoneNumber
        public string? Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
    }
}

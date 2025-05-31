using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestateCo.Domain.Entities.CoreEntities
{
    public class NotificationLog
    {
        public Guid Id { get; set; }
        public string? Recipient { get; set; }
        public string? MessageType { get; set; } // Email or SMS
        public string? MessageContent { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsSuccessful { get; set; }
    }
}

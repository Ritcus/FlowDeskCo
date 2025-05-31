using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestateCo.Domain.Entities.CoreEntities
{
    public class Document
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Guid UploadedByUserId { get; set; }
    }
}

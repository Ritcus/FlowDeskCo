using FlowDeskCo.Domain.Entities.Enums;
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
        public Guid? ClientId { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public string? Description { get; set; }
        public DocumentType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.AddDays(180);
        public Guid? UploadedByUserId { get; set; }
    }
}

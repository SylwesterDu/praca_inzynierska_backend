using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.DTOs
{
    public record ReportDTO
    {
        public Guid ReportId { get; set; }
        public Guid ArtworkId { get; set; }
        public string? ArtworkTitle { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ReportReason { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.DTOs
{
    public record ReportRequestDTO
    {
        public string? ReportReason { get; set; }
    }
}

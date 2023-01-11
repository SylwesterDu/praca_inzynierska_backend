using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.Entities
{
    public class Report
    {
        public Guid Id { get; set; }
        public Artwork? Artwork { get; set; }
        public string? ReportReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public User? ReportedBy { get; set; }
    }
}

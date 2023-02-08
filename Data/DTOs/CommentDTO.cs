using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.DTOs
{
    public class CommentDTO
    {
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatorName { get; set; }
        public Guid CreatorId { get; set; }
        public int? rating { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public Artwork? Artwork { get; set; }
        public User? Creator { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; init; }
        public int? Rating { get; set; }
    }
}

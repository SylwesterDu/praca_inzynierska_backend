using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.DTOs
{
    public record UserDetailsDTO
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public List<StatsPerArtworkTypeDTO>? Stats { get; set; }
        public string? Avatar { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Misc;

namespace praca_inzynierska_backend.Data.DTOs
{
    public record StatsPerArtworkTypeDTO
    {
        public ArtType ArtType { get; set; }
        public long Count { get; set; }
    }
}

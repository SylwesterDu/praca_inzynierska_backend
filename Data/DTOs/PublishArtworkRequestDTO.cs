using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_praca_inzynierska_backend.Misc;

namespace praca_inzynierska_backend.Data.DTOs
{
    public class PublishArtworkRequestDTO
    {
        public string? Title { get; init; }
        public string? Description { get; set; }
        public ArtType ArtType { get; init; }
        public List<string>? Tags { get; init; }
        public List<string>? Genres { get; init; }
    }
}

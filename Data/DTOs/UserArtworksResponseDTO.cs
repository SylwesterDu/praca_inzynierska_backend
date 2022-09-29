using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.DTOs
{
    public record UserArtworksResponseDTO
    {
        public List<ArtworkDTO>? Artworks { get; set; }
    }
}
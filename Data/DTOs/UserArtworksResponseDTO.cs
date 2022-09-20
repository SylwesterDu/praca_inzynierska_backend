using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.Entities;

namespace backend.Data.DTOs
{
    public record UserArtworksResponseDTO
    {
        public List<ArtworkDTO>? Artworks { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.DTOs
{
    public record StatsDTO
    {
        public object? ArtworksCount { get; set; }
        public object? ArtworksViewsCount { get; set; }
        public object? ArtworksCommentsCount { get; set; }
        public object? Votes { get; set; }
    }
}

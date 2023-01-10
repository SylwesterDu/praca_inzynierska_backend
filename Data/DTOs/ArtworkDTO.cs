using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Misc;

namespace praca_inzynierska_backend.Data.DTOs
{
    public record ArtworkDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public long Views { get; set; }
        public ArtType ArtType { get; set; }
        public List<string>? Genres { get; set; }
        public List<string>? Tags { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}

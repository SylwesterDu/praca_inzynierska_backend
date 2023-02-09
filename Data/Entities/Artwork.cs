using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_backend.Misc;

namespace praca_inzynierska_backend.Data.Entities
{
    public class Artwork
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public virtual User? Owner { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual List<Review>? Reviews { get; set; }
        public List<Vote>? Votes { get; set; }
        public long Views { get; set; }
        public ArtType ArtType { get; set; }
        public List<Genre>? Genres { get; set; }
        public List<Tag>? Tags { get; set; }
        public List<ArtworkFile>? Files { get; set; }
        public bool Published { get; set; }
        public bool AdultContent { get; set; }
    }
}

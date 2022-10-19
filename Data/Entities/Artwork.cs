using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_praca_inzynierska_backend.Misc;

namespace praca_inzynierska_praca_inzynierska_backend.Data.Entities
{
    public class Artwork
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public virtual User? Owner { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual List<Comment>? Comments { get; set; }
        public List<User>? UpvotedBy { get; set; }
        public List<User>? DownVotedBy { get; set; }
        public long Views { get; set; }
        public ArtType ArtType;
        public List<Genre>? Genres { get; set; }
        public List<Tag>? Tags { get; set; }
        public List<FileData>? FilesData { get; set; }
        public bool Published { get; set; }
    }
}

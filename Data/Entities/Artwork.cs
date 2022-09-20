using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Misc;

namespace backend.Data.Entities
{
    public class Artwork
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public virtual User? Owner { get; set; }
        public virtual List<Comment>? Comments { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public long Views { get; set; }
        public ArtType ArtType;
        public List<Tag>? Tags { get; set; }
    }
}
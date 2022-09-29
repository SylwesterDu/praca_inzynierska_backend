using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_praca_inzynierska_backend.Misc;

namespace praca_inzynierska_backend.Data.DTOs
{
    public record ArtworkDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        //public virtual User? Owner { get; set; }
        //public virtual List<Comment>? Comments { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public long Views { get; set; }
        public ArtType ArtType { get; set; }
        public List<Genre>? Genres { get; set; }
        public List<string>? Tags { get; set; }
    }
}
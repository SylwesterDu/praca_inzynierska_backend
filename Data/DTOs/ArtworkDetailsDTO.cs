using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.Entities;
using backend.Misc;

namespace backend.Data.DTOs
{
    public class ArtworkDetailsDTO
    {
        public List<string>? resourceUrls { get; set; }
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public virtual UserDTO? Owner { get; set; }
        //public virtual List<Comment>? Comments { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public long Views { get; set; }
        public ArtType ArtType;
        public IEnumerable<string>? Tags { get; set; }
    }
}
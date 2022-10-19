using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace praca_inzynierska_praca_inzynierska_backend.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
            Artworks = new List<Artwork>();
        }

        public List<Artwork>? Artworks { get; set; }
        public List<Artwork>? Upvotes { get; set; }
        public List<Artwork>? DownVotes { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}

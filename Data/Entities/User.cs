using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace praca_inzynierska_backend.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
            Artworks = new List<Artwork>();
        }

        public List<Artwork>? Artworks { get; set; }
        public List<Vote>? Votes { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Review>? Reviews { get; set; }
        public AvatarFile? Avatar { get; set; }
    }
}

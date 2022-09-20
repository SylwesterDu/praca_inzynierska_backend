using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace backend.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
            Artworks = new List<Artwork>();
        }
        public List<Artwork>? Artworks { get; set; }
    }
}
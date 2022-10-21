using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Misc
{
    public record Genre
    {
        public Genre(string name)
        {
            Id = new Guid();
            GenreName = name;
        }

        public Genre()
        {

        }
        public Guid Id { get; set; }
        public string? GenreName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_praca_inzynierska_backend.Data.Entities;

namespace praca_inzynierska_praca_inzynierska_backend.Misc
{
    public class Tag
    {

        public Tag(string name)
        {
            TagName = name;
            Id = new Guid();
        }

        public Tag()
        {

        }
        public Guid Id { get; set; }
        public string? TagName { get; set; }
        public Artwork? Artwork { get; set; }
    }
}
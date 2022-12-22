using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.Entities
{
    public class ArtworkFile
    {
        public Guid Id { get; set; }
        public Artwork? Artwork { get; set; }
        public string? Key { get; set; }
        public string? MimeType { get; set; }
    }
}

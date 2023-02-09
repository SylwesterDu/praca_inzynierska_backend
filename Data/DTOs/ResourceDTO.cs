using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.DTOs
{
    public record ResourceDTO
    {
        public string? Url { get; set; }
        public string? ContentType { get; set; }
    }
}

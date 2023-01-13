using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.DTOs
{
    public record VotesCountDTO
    {
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.DTOs
{
    public class AddReviewDTO
    {
        public string? content { get; set; }
        public int? rating { get; set; }
    }
}

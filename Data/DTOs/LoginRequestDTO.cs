using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Data.DTOs
{
    public record LoginRequestDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
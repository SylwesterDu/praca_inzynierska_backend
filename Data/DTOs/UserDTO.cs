using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.DTOs
{
    public class UserDTO
    {
        public string? Username { get; set; }
        public string[]? Roles { get; set; }
        public Guid Id { get; set; }
    }
}

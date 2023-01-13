using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.Entities
{
    public class AvatarFile
    {
        public Guid Id { get; set; }
        public User? user { get; set; }

        [ForeignKey("User")]
        public Guid userId { get; set; }
        public string? key { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace backend.Data.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public Role(string name) : base(name)
        {

        }
    }
}
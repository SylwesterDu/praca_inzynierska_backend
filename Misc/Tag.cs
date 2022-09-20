using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Misc
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
    }
}
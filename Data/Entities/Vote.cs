using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.Entities
{
    public class Vote
    {
        public Guid Id { get; set; }
        public User? User { get; set; }
        public int Value { get; set; }
        public Artwork? Artwork { get; set; }
        public DateTime CreatedAt { get; set; }

        public Vote()
        {
            CreatedAt = DateTime.Now;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Vote _Vote = (Vote)obj;
            return Artwork!.Id == _Vote.Artwork!.Id && User!.Id == _Vote.User!.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

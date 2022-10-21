using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace praca_inzynierska_backend.Data.Entities
{
    public class Upvote
    {
        public Guid Id { get; set; }
        public User? User { get; set; }
        public Artwork? Artwork { get; set; }
        public DateTime CreatedAt { get; set; }

        public Upvote()
        {
            CreatedAt = DateTime.Now;
        }

        public override bool Equals(object? obj)
        {


            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Upvote _upvote = (Upvote)obj;
            return Artwork!.Id == _upvote.Artwork!.Id
                && User!.Id == _upvote.User!.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
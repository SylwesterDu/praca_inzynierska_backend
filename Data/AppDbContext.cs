using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.Entities;
using backend.Misc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }


        public override DbSet<User>? Users { get; set; }
        public DbSet<Artwork>? Artworks { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Tag>? Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany<Artwork>(user => user.Artworks)
                .WithOne(artwork => artwork.Owner);

            builder.Entity<Artwork>()
                .HasMany<Comment>(artwork => artwork.Comments);

            builder.Entity<Artwork>()
                .HasMany<Tag>(artwork => artwork.Tags);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlServer("Data source=localhost; database=art_db; Trust Server Certificate=true; Trusted_Connection=false; User Id=SA; Password=zaq1@WSX");
        }

    }
}
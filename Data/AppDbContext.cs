using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_backend.Misc;

namespace praca_inzynierska_backend.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        static AppDbContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ArtType>();
        }

        public override DbSet<User>? Users { get; set; }
        public DbSet<Artwork>? Artworks { get; set; }
        public DbSet<Review>? Reviews { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<Genre>? Genres { get; set; }
        public DbSet<Vote>? Votes { get; set; }
        public DbSet<ArtworkFile>? Files { get; set; }
        public DbSet<Report>? Reports { get; set; }
        public DbSet<AvatarFile>? AvatarFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresEnum<ArtType>();

            builder
                .Entity<User>()
                .HasMany<Artwork>(user => user.Artworks)
                .WithOne(artwork => artwork.Owner);

            builder
                .Entity<Vote>()
                .HasOne<Artwork>(vote => vote.Artwork)
                .WithMany(artwork => artwork.Votes)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Vote>()
                .HasOne<User>(vote => vote.User)
                .WithMany(user => user.Votes)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Artwork>()
                .HasMany<Review>(artwork => artwork.Reviews)
                .WithOne(comment => comment.Artwork)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Artwork>()
                .HasMany<Tag>(artwork => artwork.Tags)
                .WithOne(tag => tag.Artwork)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Artwork>()
                .HasMany(artwork => artwork.Files)
                .WithOne(file => file.Artwork);

            builder.Entity<Report>().HasOne(report => report.Artwork);

            builder.Entity<Report>().HasOne(report => report.ReportedBy);

            builder.Entity<User>().HasOne(user => user.Avatar).WithOne(avatar => avatar.user);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlServer("Data source=localhost; database=art_db; Trust Server Certificate=true; Trusted_Connection=false; User Id=SA; Password=zaq1@WSX");
        }
    }
}

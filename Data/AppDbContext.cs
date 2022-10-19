using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_praca_inzynierska_backend.Misc;

namespace praca_inzynierska_backend.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public override DbSet<User>? Users { get; set; }
        public DbSet<Artwork>? Artworks { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<Genre>? Genres { get; set; }
        public DbSet<UploadProcess>? UploadProcesses { get; set; }
        public DbSet<FileData>? FilesData { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<User>()
                .HasMany<Artwork>(user => user.Artworks)
                .WithOne(artwork => artwork.Owner);

            builder
                .Entity<User>()
                .HasMany<Artwork>(user => user.Upvotes)
                .WithMany(artwork => artwork.UpvotedBy);

            builder
                .Entity<User>()
                .HasMany<Artwork>(user => user.DownVotes)
                .WithMany(artwork => artwork.DownVotedBy);

            builder
                .Entity<Artwork>()
                .HasMany<Comment>(artwork => artwork.Comments)
                .WithOne(comment => comment.Artwork)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Artwork>()
                .HasMany<Tag>(artwork => artwork.Tags)
                .WithOne(tag => tag.Artwork)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Artwork>()
                .HasMany<FileData>(artwork => artwork.FilesData)
                .WithOne(fileData => fileData.Artwork)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<UploadProcess>()
                .HasMany<FileData>(uploadProcess => uploadProcess.FilesData)
                .WithOne(fileData => fileData.UploadProcess)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlServer("Data source=localhost; database=art_db; Trust Server Certificate=true; Trusted_Connection=false; User Id=SA; Password=zaq1@WSX");
        }
    }
}

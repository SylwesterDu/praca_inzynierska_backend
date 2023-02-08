﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using praca_inzynierska_backend.Data;
using praca_inzynierska_backend.Misc;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230208155536_adult_content")]
    partial class adult_content
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "art_type", new[] { "music", "literature", "photography", "other" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.Artwork", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("AdultContent")
                        .HasColumnType("boolean");

                    b.Property<ArtType>("ArtType")
                        .HasColumnType("art_type");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<long>("Views")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Artworks");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.ArtworkFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ArtworkId")
                        .HasColumnType("uuid");

                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<string>("MimeType")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.AvatarFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("key")
                        .HasColumnType("text");

                    b.Property<Guid>("userId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("userId")
                        .IsUnique();

                    b.ToTable("AvatarFiles");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ArtworkId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<int?>("rating")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.Downvote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ArtworkId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.HasIndex("UserId");

                    b.ToTable("Downvotes");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.Report", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ArtworkId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ReportReason")
                        .HasColumnType("text");

                    b.Property<Guid?>("ReportedById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.HasIndex("ReportedById");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.Upvote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ArtworkId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.HasIndex("UserId");

                    b.ToTable("Upvotes");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("praca_inzynierska_backend.Misc.Genre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ArtworkId")
                        .HasColumnType("uuid");

                    b.Property<string>("GenreName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Misc.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ArtworkId")
                        .HasColumnType("uuid");

                    b.Property<string>("TagName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("praca_inzynierska_backend.Data.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("praca_inzynierska_backend.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("praca_inzynierska_backend.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("praca_inzynierska_backend.Data.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("praca_inzynierska_backend.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("praca_inzynierska_backend.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.Artwork", b =>
                {
                    b.HasOne("praca_inzynierska_backend.Data.Entities.User", "Owner")
                        .WithMany("Artworks")
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.ArtworkFile", b =>
                {
                    b.HasOne("praca_inzynierska_backend.Data.Entities.Artwork", "Artwork")
                        .WithMany("Files")
                        .HasForeignKey("ArtworkId");

                    b.Navigation("Artwork");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.AvatarFile", b =>
                {
                    b.HasOne("praca_inzynierska_backend.Data.Entities.User", "user")
                        .WithOne("Avatar")
                        .HasForeignKey("praca_inzynierska_backend.Data.Entities.AvatarFile", "userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.Comment", b =>
                {
                    b.HasOne("praca_inzynierska_backend.Data.Entities.Artwork", "Artwork")
                        .WithMany("Comments")
                        .HasForeignKey("ArtworkId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("praca_inzynierska_backend.Data.Entities.User", "Creator")
                        .WithMany("Comments")
                        .HasForeignKey("CreatorId");

                    b.Navigation("Artwork");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.Downvote", b =>
                {
                    b.HasOne("praca_inzynierska_backend.Data.Entities.Artwork", "Artwork")
                        .WithMany("Downvotes")
                        .HasForeignKey("ArtworkId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("praca_inzynierska_backend.Data.Entities.User", "User")
                        .WithMany("Downvotes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Artwork");

                    b.Navigation("User");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.Report", b =>
                {
                    b.HasOne("praca_inzynierska_backend.Data.Entities.Artwork", "Artwork")
                        .WithMany()
                        .HasForeignKey("ArtworkId");

                    b.HasOne("praca_inzynierska_backend.Data.Entities.User", "ReportedBy")
                        .WithMany()
                        .HasForeignKey("ReportedById");

                    b.Navigation("Artwork");

                    b.Navigation("ReportedBy");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.Upvote", b =>
                {
                    b.HasOne("praca_inzynierska_backend.Data.Entities.Artwork", "Artwork")
                        .WithMany("Upvotes")
                        .HasForeignKey("ArtworkId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("praca_inzynierska_backend.Data.Entities.User", "User")
                        .WithMany("Upvotes")
                        .HasForeignKey("UserId");

                    b.Navigation("Artwork");

                    b.Navigation("User");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Misc.Genre", b =>
                {
                    b.HasOne("praca_inzynierska_backend.Data.Entities.Artwork", null)
                        .WithMany("Genres")
                        .HasForeignKey("ArtworkId");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Misc.Tag", b =>
                {
                    b.HasOne("praca_inzynierska_backend.Data.Entities.Artwork", "Artwork")
                        .WithMany("Tags")
                        .HasForeignKey("ArtworkId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Artwork");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.Artwork", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Downvotes");

                    b.Navigation("Files");

                    b.Navigation("Genres");

                    b.Navigation("Tags");

                    b.Navigation("Upvotes");
                });

            modelBuilder.Entity("praca_inzynierska_backend.Data.Entities.User", b =>
                {
                    b.Navigation("Artworks");

                    b.Navigation("Avatar");

                    b.Navigation("Comments");

                    b.Navigation("Downvotes");

                    b.Navigation("Upvotes");
                });
#pragma warning restore 612, 618
        }
    }
}

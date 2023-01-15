using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using praca_inzynierska_backend.Services.AccountService;
using praca_inzynierska_backend.Data.DTOs;
using Microsoft.AspNetCore.Identity;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_backend.Data;
using Microsoft.EntityFrameworkCore;
using praca_inzynierska_backend.Repositories.AccountRepository;
using praca_inzynierska_backend.Repositories.ArtworksRepository;
using praca_inzynierska_backend.Services.UploadService;
using praca_inzynierska_backend.Services.CloudflareFileService;
using praca_inzynierska_backend.Misc;
using praca_inzynierska_backend.Services.ArtworksService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace praca_inzynierska_backend.UnitTests
{
    public class UnitTests
    {
        private DbContextOptions<AppDbContext> dbContextOptions =
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "art_db_test")
                .Options;

        AppDbContext context;

        public UnitTests()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();
        }

        [Fact]
        public async void AddArtwork_OwnerIdShouldBeEqual()
        {
            Guid userId = Guid.NewGuid();
            User user = new User()
            {
                Id = userId,
                UserName = "test",
                Email = "test@test.pl",
            };
            context.Users!.Add(user);

            Guid artworkId = Guid.NewGuid();
            Artwork artwork = new Artwork()
            {
                Id = artworkId,
                ArtType = ArtType.MUSIC,
                Owner = user,
                Genres = new List<Genre>() { new Genre("rock") },
                Tags = new List<Tag>() { new Tag("testowy tag") },
                Views = 0
            };

            context.Artworks!.Add(artwork);

            context.SaveChanges();

            AccountRepository accountRepository = new AccountRepository(context);
            ArtworksRepository artworksRepository = new ArtworksRepository(context);

            ArtworksService artworksService = new ArtworksService(
                artworksRepository,
                accountRepository,
                null,
                null
            );

            ArtworkDetailsDTO dto = await artworksService.GetArtworkDetails(artworkId);

            Assert.Equal(dto.Owner!.Id, userId);
        }

        [Fact]
        public async void GetArtworkCount_ShuldCountPublishedArtworks()
        {
            Guid userId = Guid.NewGuid();
            User user = new User()
            {
                Id = userId,
                UserName = "test",
                Email = "test@test.pl",
                Artworks = new List<Artwork>()
            };

            Artwork artwork1 = new Artwork()
            {
                Id = Guid.NewGuid(),
                ArtType = ArtType.MUSIC,
                Genres = new List<Genre>() { new Genre("rock") },
                Tags = new List<Tag>() { new Tag("testowy tag") },
                Views = 0,
                Published = true
            };

            Artwork artwork2 = new Artwork()
            {
                Id = Guid.NewGuid(),
                ArtType = ArtType.MUSIC,
                Genres = new List<Genre>() { new Genre("rock") },
                Tags = new List<Tag>() { new Tag("testowy tag") },
                Views = 0,
                Published = true
            };

            Artwork artwork3 = new Artwork()
            {
                Id = Guid.NewGuid(),
                ArtType = ArtType.LITERATURE,
                Genres = new List<Genre>() { new Genre("poemat") },
                Tags = new List<Tag>() { new Tag("po polsku") },
                Views = 0
            };

            user.Artworks.Add(artwork1);
            user.Artworks.Add(artwork2);
            user.Artworks.Add(artwork3);

            context.Users!.Add(user);

            context.SaveChanges();

            AccountRepository accountRepository = new AccountRepository(context);
            ArtworksRepository artworksRepository = new ArtworksRepository(context);

            ArtworksService artworksService = new ArtworksService(
                artworksRepository,
                accountRepository,
                null,
                null
            );

            List<ArtworkDTO> dtos = await artworksService.GetUserArtworks(userId);

            Assert.Equal(2, dtos.Count());
        }

        [Fact]
        public async void GetPopularArtworks_ShouldReturnArtworksInCurrentWeek()
        {
            Mock<ICloudflareFileService> cloudflareService = new Mock<ICloudflareFileService>();
            cloudflareService
                .Setup(service => service.GetFileUrl(It.IsAny<string>()))
                .Returns("test");

            ArtworkFile file = new ArtworkFile()
            {
                Id = Guid.NewGuid(),
                Key = "zaqwsx",
                MimeType = "image/png"
            };

            Guid userId = Guid.NewGuid();
            User user = new User()
            {
                Id = userId,
                UserName = "test",
                Email = "test@test.pl",
                Artworks = new List<Artwork>()
            };

            Artwork artwork1 = new Artwork()
            {
                Id = Guid.NewGuid(),
                ArtType = ArtType.MUSIC,
                Genres = new List<Genre>() { new Genre("rock") },
                Tags = new List<Tag>() { new Tag("testowy tag") },
                Views = 0,
                Published = true,
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                Files = new List<ArtworkFile>() { file }
            };

            Artwork artwork2 = new Artwork()
            {
                Id = Guid.NewGuid(),
                ArtType = ArtType.MUSIC,
                Genres = new List<Genre>() { new Genre("rock") },
                Tags = new List<Tag>() { new Tag("testowy tag") },
                Views = 0,
                Published = true,
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(5)),
                Files = new List<ArtworkFile>() { file }
            };

            Artwork artwork3 = new Artwork()
            {
                Id = Guid.NewGuid(),
                ArtType = ArtType.LITERATURE,
                Genres = new List<Genre>() { new Genre("poemat") },
                Tags = new List<Tag>() { new Tag("po polsku") },
                Views = 0,
                Published = true,
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
                Files = new List<ArtworkFile>() { file }
            };

            user.Artworks.Add(artwork1);
            user.Artworks.Add(artwork2);
            user.Artworks.Add(artwork3);

            context.Users!.Add(user);

            context.SaveChanges();

            AccountRepository accountRepository = new AccountRepository(context);
            ArtworksRepository artworksRepository = new ArtworksRepository(context);

            ArtworksService artworksService = new ArtworksService(
                artworksRepository,
                accountRepository,
                cloudflareService.Object,
                null
            );

            List<ArtworkDTO> dtos = await artworksService.GetPopularArtworks(ArtType.MUSIC);

            Assert.Equal(2, dtos.Count());
        }

        [Fact]
        public async void AddReport_ShouldByReturnedToAdmin()
        {
            Mock<ICloudflareFileService> cloudflareService = new Mock<ICloudflareFileService>();
            cloudflareService
                .Setup(service => service.GetFileUrl(It.IsAny<string>()))
                .Returns("test");

            ArtworkFile file = new ArtworkFile()
            {
                Id = Guid.NewGuid(),
                Key = "zaqwsx",
                MimeType = "image/png"
            };

            Guid userId = Guid.NewGuid();
            User user = new User()
            {
                Id = userId,
                UserName = "test",
                Email = "test@test.pl",
                Artworks = new List<Artwork>()
            };

            Guid user2Id = Guid.NewGuid();
            User user2 = new User()
            {
                Id = user2Id,
                UserName = "test2",
                Email = "test2@test.pl",
                Artworks = new List<Artwork>()
            };

            Guid artworkId = Guid.NewGuid();

            Artwork artwork = new Artwork()
            {
                Id = artworkId,
                ArtType = ArtType.MUSIC,
                Genres = new List<Genre>() { new Genre("rock") },
                Tags = new List<Tag>() { new Tag("testowy tag") },
                Views = 0,
                Published = true,
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                Files = new List<ArtworkFile>() { file },
                Owner = user
            };

            Report report = new Report()
            {
                Artwork = artwork,
                CreatedAt = DateTime.Now,
                Id = Guid.NewGuid(),
                ReportedBy = user2,
                ReportReason = "zła kategoria"
            };

            context.Users!.Add(user);
            context.Users!.Add(user2);
            user.Artworks.Add(artwork);

            context.Reports!.Add(report);

            context.SaveChanges();

            AccountRepository accountRepository = new AccountRepository(context);
            ArtworksRepository artworksRepository = new ArtworksRepository(context);

            ArtworksService artworksService = new ArtworksService(
                artworksRepository,
                accountRepository,
                cloudflareService.Object,
                null
            );

            AccountService accountService = new AccountService(
                null,
                accountRepository,
                null,
                null,
                artworksRepository,
                cloudflareService.Object
            );

            List<ReportDTO> dtos = await artworksService.GetReports();

            Assert.Equal(1, dtos.Count());
            Assert.Equal("zła kategoria", dtos[0].ReportReason);
            Assert.Equal(artworkId, dtos[0].ArtworkId);
        }

        [Fact]
        public async void AddArtworks_SholuldReturnProperStats()
        {
            Mock<ICloudflareFileService> cloudflareService = new Mock<ICloudflareFileService>();
            cloudflareService
                .Setup(service => service.GetFileUrl(It.IsAny<string>()))
                .Returns("test");

            ArtworkFile file = new ArtworkFile()
            {
                Id = Guid.NewGuid(),
                Key = "zaqwsx",
                MimeType = "image/png"
            };

            Guid userId = Guid.NewGuid();
            User user = new User()
            {
                Id = userId,
                UserName = "test",
                Email = "test@test.pl",
                Artworks = new List<Artwork>()
            };

            Guid artworkId = Guid.NewGuid();
            Artwork artwork = new Artwork()
            {
                Id = artworkId,
                ArtType = ArtType.MUSIC,
                Genres = new List<Genre>() { new Genre("rock") },
                Tags = new List<Tag>() { new Tag("testowy tag") },
                Views = 0,
                Published = true,
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                Files = new List<ArtworkFile>() { file },
                Owner = user
            };

            Artwork artwork2 = new Artwork()
            {
                Id = Guid.NewGuid(),
                ArtType = ArtType.PHOTOGRAPHY,
                Genres = new List<Genre>() { new Genre("natura") },
                Tags = new List<Tag>() { new Tag("testowy tag") },
                Views = 0,
                Published = true,
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                Files = new List<ArtworkFile>() { file },
                Owner = user
            };

            Artwork artwork3 = new Artwork()
            {
                Id = Guid.NewGuid(),
                ArtType = ArtType.OTHER,
                Genres = new List<Genre>() { new Genre("rock") },
                Tags = new List<Tag>() { new Tag("testowy tag") },
                Views = 0,
                Published = true,
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                Files = new List<ArtworkFile>() { file },
                Owner = user
            };

            context.Users!.Add(user);
            user.Artworks.Add(artwork);
            user.Artworks.Add(artwork2);
            user.Artworks.Add(artwork3);

            context.SaveChanges();

            AccountRepository accountRepository = new AccountRepository(context);
            ArtworksRepository artworksRepository = new ArtworksRepository(context);

            ArtworksService artworksService = new ArtworksService(
                artworksRepository,
                accountRepository,
                cloudflareService.Object,
                null
            );

            AccountService accountService = new AccountService(
                null,
                accountRepository,
                null,
                null,
                artworksRepository,
                cloudflareService.Object
            );

            UserDetailsDTO dto = await accountService.GetUserDetails(userId);

            Assert.Equal(3, dto.Stats!.Count);
            Assert.Equal(1, dto.Stats[0].Count);
            Assert.Equal(ArtType.OTHER, dto.Stats[2].ArtType);
        }

        [Fact]
        public async void SearchArtwork_shouldReturnCorrectTags()
        {
            Mock<ICloudflareFileService> cloudflareService = new Mock<ICloudflareFileService>();
            cloudflareService
                .Setup(service => service.GetFileUrl(It.IsAny<string>()))
                .Returns("test");

            ArtworkFile file = new ArtworkFile()
            {
                Id = Guid.NewGuid(),
                Key = "zaqwsx",
                MimeType = "image/png"
            };

            Guid userId = Guid.NewGuid();
            User user = new User()
            {
                Id = userId,
                UserName = "test",
                Email = "test@test.pl",
                Artworks = new List<Artwork>()
            };

            Guid artworkId = Guid.NewGuid();
            Artwork artwork = new Artwork()
            {
                Id = artworkId,
                Title = "piosenka rock",
                ArtType = ArtType.MUSIC,
                Genres = new List<Genre>() { new Genre("rock") },
                Tags = new List<Tag>() { new Tag("pl") },
                Views = 0,
                Published = true,
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                Files = new List<ArtworkFile>() { file },
                Owner = user
            };

            Artwork artwork2 = new Artwork()
            {
                Id = Guid.NewGuid(),
                Title = "piosenka pop",
                ArtType = ArtType.PHOTOGRAPHY,
                Genres = new List<Genre>() { new Genre("natura") },
                Tags = new List<Tag>() { new Tag("eng") },
                Views = 0,
                Published = true,
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                Files = new List<ArtworkFile>() { file },
                Owner = user
            };

            Artwork artwork3 = new Artwork()
            {
                Id = Guid.NewGuid(),
                Title = "popkultura",
                ArtType = ArtType.OTHER,
                Genres = new List<Genre>() { new Genre("rock") },
                Tags = new List<Tag>() { new Tag("de") },
                Views = 0,
                Published = true,
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                Files = new List<ArtworkFile>() { file },
                Owner = user
            };

            context.Users!.Add(user);
            user.Artworks.Add(artwork);
            user.Artworks.Add(artwork2);
            user.Artworks.Add(artwork3);

            context.SaveChanges();

            AccountRepository accountRepository = new AccountRepository(context);
            ArtworksRepository artworksRepository = new ArtworksRepository(context);

            ArtworksService artworksService = new ArtworksService(
                artworksRepository,
                accountRepository,
                cloudflareService.Object,
                null
            );

            AccountService accountService = new AccountService(
                null,
                accountRepository,
                null,
                null,
                artworksRepository,
                cloudflareService.Object
            );

            List<ArtworkDTO> artworks = await artworksService.SearchArtworks(
                null,
                null,
                null,
                "pl"
            );

            Assert.Equal(1, artworks.Count());
            Assert.Equal("pl", artworks[0].Tags!.First());
        }

        [Fact]
        public async void SearchArtwork_shouldReturnCorrectTitle()
        {
            Mock<ICloudflareFileService> cloudflareService = new Mock<ICloudflareFileService>();
            cloudflareService
                .Setup(service => service.GetFileUrl(It.IsAny<string>()))
                .Returns("test");

            ArtworkFile file = new ArtworkFile()
            {
                Id = Guid.NewGuid(),
                Key = "zaqwsx",
                MimeType = "image/png"
            };

            Guid userId = Guid.NewGuid();
            User user = new User()
            {
                Id = userId,
                UserName = "test",
                Email = "test@test.pl",
                Artworks = new List<Artwork>()
            };

            Guid artworkId = Guid.NewGuid();
            Artwork artwork = new Artwork()
            {
                Id = artworkId,
                Title = "piosenka rock",
                ArtType = ArtType.MUSIC,
                Genres = new List<Genre>() { new Genre("rock") },
                Tags = new List<Tag>() { new Tag("testowy tag") },
                Views = 0,
                Published = true,
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                Files = new List<ArtworkFile>() { file },
                Owner = user
            };

            Artwork artwork2 = new Artwork()
            {
                Id = Guid.NewGuid(),
                Title = "piosenka pop",
                ArtType = ArtType.PHOTOGRAPHY,
                Genres = new List<Genre>() { new Genre("natura") },
                Tags = new List<Tag>() { new Tag("testowy tag") },
                Views = 0,
                Published = true,
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                Files = new List<ArtworkFile>() { file },
                Owner = user
            };

            Artwork artwork3 = new Artwork()
            {
                Id = Guid.NewGuid(),
                Title = "popkultura",
                ArtType = ArtType.OTHER,
                Genres = new List<Genre>() { new Genre("rock") },
                Tags = new List<Tag>() { new Tag("testowy tag") },
                Views = 0,
                Published = true,
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                Files = new List<ArtworkFile>() { file },
                Owner = user
            };

            context.Users!.Add(user);
            user.Artworks.Add(artwork);
            user.Artworks.Add(artwork2);
            user.Artworks.Add(artwork3);

            context.SaveChanges();

            AccountRepository accountRepository = new AccountRepository(context);
            ArtworksRepository artworksRepository = new ArtworksRepository(context);

            ArtworksService artworksService = new ArtworksService(
                artworksRepository,
                accountRepository,
                cloudflareService.Object,
                null
            );

            List<ArtworkDTO> artworks1 = await artworksService.SearchArtworks(
                "piosenka",
                null,
                null,
                null
            );
            List<ArtworkDTO> artworks2 = await artworksService.SearchArtworks(
                "pop",
                null,
                null,
                null
            );

            Assert.Equal(2, artworks1.Count());
            Assert.Equal(2, artworks2.Count());
        }
    }
}

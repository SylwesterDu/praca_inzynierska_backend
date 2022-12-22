using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_backend.Misc;
using praca_inzynierska_backend.Repositories.ArtworksRepository;
using praca_inzynierska_backend.Services.AccountService;
using praca_inzynierska_backend.Services.CloudflareFileService;

namespace praca_inzynierska_backend.Services.UploadService
{
    public class UploadService : IUploadService
    {
        private readonly IAccountService _accountService;
        private readonly IArtworksRepository _artworksRepository;
        private readonly ICloudflareFileService _cloudflareFileService;
        private readonly IConfiguration _configuration;

        public UploadService(
            IAccountService accountService,
            IConfiguration configuration,
            IArtworksRepository artworksRepository,
            ICloudflareFileService cloudflareFileService
        )
        {
            _accountService = accountService;
            _configuration = configuration;
            _artworksRepository = artworksRepository;
            _cloudflareFileService = cloudflareFileService;
        }

        public async Task<UploadProcessDTO> CreateArtwork(string token)
        {
            User user = await _accountService.GetUserByToken(token);
            Guid artworkId = Guid.NewGuid();
            Artwork artwork = new Artwork()
            {
                Id = artworkId,
                Published = false,
                Owner = user,
                Upvotes = new List<Upvote>(),
                Downvotes = new List<Downvote>(),
                Views = 0
            };
            await _artworksRepository.AddArtwork(artwork);
            return new UploadProcessDTO() { Id = artworkId, };
        }

        public async Task<bool> UploadFile(string token, IFormFile formFile, Guid id)
        {
            User user = await _accountService.GetUserByToken(token);
            Artwork artwork = await _artworksRepository.GetArtworkById(id);

            string key = await _cloudflareFileService.UploadFile(id, formFile);

            ArtworkFile file = new ArtworkFile()
            {
                Id = new Guid(),
                Artwork = artwork,
                Key = key,
                MimeType = formFile.ContentType
            };

            artwork.Files!.Add(file);

            await _artworksRepository.SaveFile(artwork);
            return true;
        }

        public async Task<bool> PublishArtWork(
            string token,
            Guid id,
            PublishArtworkRequestDTO publishArtworkRequestDTO
        )
        {
            User user = await _accountService.GetUserByToken(token);

            Artwork artwork = await _artworksRepository.GetArtworkById(id);

            artwork.ArtType = publishArtworkRequestDTO.ArtType;
            artwork.Comments = new List<Comment>();
            artwork.CreatedAt = DateTime.Now;
            artwork.Title = publishArtworkRequestDTO.Title;
            artwork.Description = publishArtworkRequestDTO.Description;
            artwork.Genres = publishArtworkRequestDTO.Genres!
                .Select(genre => new Genre(genre) { Id = new Guid() })
                .ToList();
            artwork.Tags = publishArtworkRequestDTO.Tags!
                .Select(tag => new Tag(tag) { Id = new Guid() })
                .ToList();
            artwork.Published = true;

            await _artworksRepository.SaveArtwork(artwork);

            return true;
        }
    }
}

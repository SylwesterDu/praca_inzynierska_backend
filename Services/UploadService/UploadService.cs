using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_backend.Misc;
using praca_inzynierska_backend.Repositories.FilesRepository;
using praca_inzynierska_backend.Services.AccountService;

namespace praca_inzynierska_backend.Services.UploadService
{
    public class UploadService : IUploadService
    {
        private readonly IAccountService _accountService;
        private readonly IFilesRepository _filesRepository;
        private readonly IConfiguration _configuration;

        public UploadService(
            IAccountService accountService,
            IFilesRepository filesRepository,
            IConfiguration configuration
        )
        {
            _accountService = accountService;
            _filesRepository = filesRepository;
            _configuration = configuration;
        }

        public async Task<UploadProcessDTO> CreateUploadProcess(string token)
        {
            User user = await _accountService.GetUserByToken(token);
            UploadProcess process =
                new()
                {
                    Id = new Guid(),
                    CreatedAt = DateTime.Now,
                    Uploader = user
                };
            await _filesRepository.AddUploadProcess(process);
            return new UploadProcessDTO()
            {
                CreatedAt = process.CreatedAt,
                Id = process.Id,
                Uploader = process.Uploader.Id
            };
        }

        public async Task<bool> UploadFile(string token, IFormFile formFile, Guid id)
        {
            User user = await _accountService.GetUserByToken(token);
            UploadProcess process = await _filesRepository.GetUploadProcessById(id);
            if (process.Uploader != user)
            {
                return false;
            }

            FileData fileData =
                new()
                {
                    UploadProcess = process,
                    FileName = Path.GetRandomFileName(),
                    Id = new Guid(),
                    Path = _configuration["FilesPath"],
                };

            if (!Directory.Exists(_configuration["FilesPath"]))
            {
                Directory.CreateDirectory(_configuration["FilesPath"]);
            }

            using (var stream = System.IO.File.Create(fileData.Path + "/" + fileData.FileName))
            {
                await formFile.CopyToAsync(stream);
            }

            await _filesRepository.AddFile(fileData);
            return true;
        }

        public async Task<bool> PublishArtWork(
            string token,
            Guid id,
            PublishArtworkRequestDTO publishArtworkRequestDTO
        )
        {
            User user = await _accountService.GetUserByToken(token);

            UploadProcess process = await _filesRepository.GetUploadProcessById(id);
            if (user != process.Uploader)
            {
                return false;
            }

            Artwork artwork = new Artwork()
            {
                ArtType = publishArtworkRequestDTO.ArtType,
                Title = publishArtworkRequestDTO.Title,
                Tags = publishArtworkRequestDTO.Tags!.Select(tag => new Tag(tag)).ToList(),
                Genres = publishArtworkRequestDTO.Genres!
                    .Select(genre => new Genre(genre))
                    .ToList(),
                Id = process.Id,
                FilesData = process.FilesData,
                Views = 0,
                Upvotes = new List<Upvote>(),
                // DownVotedBy = new List<User>(),
                Owner = user,
                Comments = new List<Comment>(),
                Published = true,
                CreatedAt = DateTime.Now,
                Description = publishArtworkRequestDTO.Description
            };

            await _filesRepository.AddArtwork(artwork);
            await _filesRepository.SetArtworkToFiles(process, artwork.Id);
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Repositories.AccountRepository;
using praca_inzynierska_backend.Repositories.ArtworksRepository;
using praca_inzynierska_praca_inzynierska_backend.Data.Entities;

namespace praca_inzynierska_backend.Services.ArtworksService
{
    public class ArtworksService : IArtworksService
    {
        private IArtworksRepository _artworksRepository;
        private IAccountRepository _accountRepository;

        public ArtworksService(IArtworksRepository artworksRepository, IAccountRepository accountRepository)
        {
            _artworksRepository = artworksRepository;
            _accountRepository = accountRepository;
        }

        public async Task AddComment(string token, Guid id, string content)
        {
            User user = await _accountRepository.GetUserByToken(token);
            Artwork artwork = await _artworksRepository.GetArtworkById(id);
            Comment comment = new Comment()
            {
                Artwork = artwork,
                Content = content,
                CreatedAt = DateTime.Now,
                Creator = user
            };

            await _artworksRepository.AddComment(comment);
        }

        public async Task<IEnumerable<CommentDTO>> GetArtworkComments(Guid id)
        {
            IEnumerable<Comment> comments = await _artworksRepository.GetArtworkComments(id);

            if (comments is null)
            {
                return null!;
            }

            return comments.Select(comment => new CommentDTO()
            {
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                CreatorId = comment.Creator!.Id,
                CreatorName = comment.Creator.UserName

            });
        }

        public async Task<ArtworkDetailsDTO> GetArtworkDetails(Guid id)
        {

            Artwork? artwork = await _artworksRepository.GetArtworkById(id);
            if (artwork is null)
            {
                return null!;
            }


            return new ArtworkDetailsDTO()
            {
                ArtType = artwork!.ArtType,
                DownVotes = artwork.DownVotes,
                UpVotes = artwork.UpVotes,
                Id = artwork.Id,
                Owner = new UserDTO()
                {
                    Username = artwork.Owner!.UserName,
                    Id = artwork.Owner!.Id
                },
                resourceUrls = artwork.FilesData!.Select(filedata => filedata.FileName).ToList()!,
                Tags = artwork.Tags!.Select(tag => tag.TagName)!,
                Genres = artwork.Genres!.Select(genre => genre.GenreName).ToList()!,
                Title = artwork.Title,
                Views = artwork.Views
            };
        }

        public async Task<UserArtworksResponseDTO> GetUserArtworks(Guid id)
        {
            List<Artwork> artworks = await _artworksRepository.GetUserArtworks(id);

            UserArtworksResponseDTO dto = new UserArtworksResponseDTO();

            dto.Artworks = artworks.Select(artwork => new ArtworkDTO()
            {
                Id = artwork.Id,
                Title = artwork.Title,
                ArtType = artwork.ArtType,
                Genres = artwork.Genres,
                Tags = artwork.Tags!.Select(tag => tag.TagName).ToList()!,
                UpVotes = artwork.UpVotes,
                DownVotes = artwork.DownVotes,
                Views = artwork.Views
            }).ToList();
            return dto;

        }
    }
}
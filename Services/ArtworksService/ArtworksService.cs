using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.DTOs;
using backend.Data.Entities;
using backend.Repositories.AccountRepository;
using backend.Repositories.ArtworksRepository;

namespace backend.Services.ArtworksService
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
            User user = await _accountRepository.getUserByToken(token);
            Artwork artwork = await _artworksRepository.getArtworkById(id);
            Comment comment = new Comment()
            {
                Artwork = artwork,
                Content = content,
                CreatedAt = DateTime.Now,
                Creator = user
            };

            await _artworksRepository.addComment(comment);
        }

        public async Task<IEnumerable<CommentDTO>> getArtworkComments(Guid id)
        {
            IEnumerable<Comment> comments = await _artworksRepository.getArtworkComments(id);

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

        public async Task<ArtworkDetailsDTO> getArtworkDetails(Guid id)
        {

            Artwork? artwork = await _artworksRepository.getArtworkById(id);
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
                resourceUrls = new List<string>(), //TODO: url do zasobów
                Tags = artwork.Tags!.Select(tag => tag.TagName)!,
                Title = artwork.Title,
                Views = artwork.Views
            };
        }

        public async Task<UserArtworksResponseDTO> GetUserArtworks(Guid id)
        {
            return await _artworksRepository.GetUserArtworks(id);
        }
    }
}
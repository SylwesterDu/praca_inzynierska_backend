using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_backend.Repositories.AccountRepository;
using praca_inzynierska_backend.Repositories.ArtworksRepository;

namespace praca_inzynierska_backend.Services.ArtworksService
{
    public class ArtworksService : IArtworksService
    {
        private IArtworksRepository _artworksRepository;
        private IAccountRepository _accountRepository;

        public ArtworksService(
            IArtworksRepository artworksRepository,
            IAccountRepository accountRepository
        )
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

        public async Task<bool> DeleteArtwork(string token, Guid id)
        {
            User? user = await _accountRepository.GetUserByToken(token);
            Artwork? artwork = await _artworksRepository.GetArtworkById(id);

            if (artwork is null)
            {
                return false;
            }

            if (artwork.Owner != user)
            {
                return false;
            }

            await _artworksRepository.DeleteArtwork(artwork);

            return true;
        }

        public async Task<bool> DownvoteArtwork(string token, Guid id)
        {
            Guid userId = _accountRepository.GetUserIdFromToken(token);
            User? user = await _accountRepository.GetUserWithUpvotesById(userId);
            Artwork? artwork = await _artworksRepository.GetArtworkWithUpvotesById(id);

            Downvote? downvote = await _artworksRepository.GetDownvote(userId, id);

            if (downvote is not null)
            {
                return false;
            }

            Upvote upvote = await _artworksRepository.GetUpvote(userId, id);
            if (upvote is not null)
            {
                await _artworksRepository.DeleteUpvote(upvote);
            }



            downvote = new Downvote()
            {
                Artwork = artwork,
                User = user,
                Id = new Guid()
            };
            bool success = await _artworksRepository.DownvoteArtwork(downvote);

            return success;
        }

        public async Task<IEnumerable<CommentDTO>> GetArtworkComments(Guid id)
        {
            IEnumerable<Comment> comments = await _artworksRepository.GetArtworkComments(id);

            if (comments is null)
            {
                return null!;
            }

            return comments.Select(
                comment =>
                    new CommentDTO()
                    {
                        Content = comment.Content,
                        CreatedAt = comment.CreatedAt,
                        CreatorId = comment.Creator!.Id,
                        CreatorName = comment.Creator.UserName
                    }
            );
        }

        public async Task<ArtworkDetailsDTO> GetArtworkDetails(Guid id)
        {
            Artwork? artwork = await _artworksRepository.GetArtworkById(id);
            int upvotesCount = await _artworksRepository.GetArtworkUpvotesCount(id);
            int downvotesCount = await _artworksRepository.GetArtworkDownvotesCount(id);
            if (artwork is null)
            {
                return null!;
            }

            return new ArtworkDetailsDTO()
            {
                ArtType = artwork!.ArtType,
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
                Description = artwork.Description,
                Views = artwork.Views,
                Upvotes = upvotesCount,
                Downvotes = downvotesCount
            };
        }

        public async Task<List<ArtworkDTO>> GetUserArtworks(Guid id)
        {
            List<Artwork> artworks = await _artworksRepository.GetUserArtworks(id);
            List<ArtworkDTO> artworkDTOs = artworks
                .Select(
                    artwork =>
                        new ArtworkDTO
                        {
                            ArtType = artwork.ArtType,
                            Genres = artwork.Genres!.Select(genre => genre.GenreName).ToList()!,
                            Id = artwork.Id,
                            Tags = artwork.Tags!.Select(tag => tag.TagName).ToList()!,
                            Title = artwork.Title,
                            Views = artwork.Views
                        }
                )
                .ToList();

            return artworkDTOs;
        }

        public async Task<bool> UpvoteArtwork(string token, Guid id)
        {
            Guid userId = _accountRepository.GetUserIdFromToken(token);

            User? user = await _accountRepository.GetUserWithUpvotesById(userId);
            Artwork? artwork = await _artworksRepository.GetArtworkWithUpvotesById(id);

            Upvote? upvote = await _artworksRepository.GetUpvote(userId, id);
            if (upvote is not null)
            {
                return false;
            }

            Downvote? downvote = await _artworksRepository.GetDownvote(userId, id);
            if (downvote is not null)
            {
                await _artworksRepository.DeleteDownvote(downvote);
            }

            upvote = new Upvote()
            {
                Artwork = artwork,
                User = user,
                Id = new Guid()
            };
            bool success = await _artworksRepository.UpvoteArtwork(upvote);

            return success;
        }
    }
}

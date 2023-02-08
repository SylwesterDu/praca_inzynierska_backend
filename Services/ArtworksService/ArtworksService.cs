using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_backend.Misc;
using praca_inzynierska_backend.Repositories.AccountRepository;
using praca_inzynierska_backend.Repositories.ArtworksRepository;
using praca_inzynierska_backend.Services.CloudflareFileService;

namespace praca_inzynierska_backend.Services.ArtworksService
{
    public class ArtworksService : IArtworksService
    {
        private IArtworksRepository _artworksRepository;
        private IAccountRepository _accountRepository;
        private ICloudflareFileService _cloudflareFileService;
        private UserManager<User> _userManager;

        public ArtworksService(
            IArtworksRepository artworksRepository,
            IAccountRepository accountRepository,
            ICloudflareFileService cloudflareFileService,
            UserManager<User> userManager
        )
        {
            _artworksRepository = artworksRepository;
            _accountRepository = accountRepository;
            _cloudflareFileService = cloudflareFileService;
            _userManager = userManager;
        }

        public async Task AddComment(string token, Guid id, AddCommentDTO dto)
        {
            User user = await _accountRepository.GetUserByToken(token);
            Artwork? artwork = await _artworksRepository.GetArtworkById(id)!;
            Comment comment = new Comment()
            {
                Artwork = artwork,
                Content = dto.content,
                CreatedAt = DateTime.Now,
                rating = dto.rating,
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

            List<string>? userRoles = await _userManager.GetRolesAsync(user) as List<string>;

            if ((user.Id != artwork.Owner!.Id) && !userRoles!.Contains("admin"))
            {
                return false;
            }

            await _artworksRepository.DeleteArtwork(artwork);

            return true;
        }

        public async Task DeleteReport(Guid reportId)
        {
            Report report = await _artworksRepository.GetReportById(reportId);
            await _artworksRepository.DeleteReport(report);
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

        public async Task<List<CommentDTO>> GetArtworkComments(Guid id)
        {
            IEnumerable<Comment> comments = await _artworksRepository.GetArtworkComments(id);

            if (comments is null)
            {
                return null!;
            }

            return comments
                .Select(
                    comment =>
                        new CommentDTO()
                        {
                            Content = comment.Content,
                            CreatedAt = comment.CreatedAt,
                            CreatorId = comment.Creator!.Id,
                            CreatorName = comment.Creator.UserName,
                            rating = comment.rating
                        }
                )
                .ToList();
        }

        public async Task<ArtworkDetailsDTO> GetArtworkDetails(Guid id, string token)
        {
            User user = await _accountRepository.GetUserByToken(token);
            Artwork? artwork = await _artworksRepository.GetArtworkById(id)!;
            if (artwork.AdultContent)
            {
                if (user is null)
                {
                    return null!;
                }
                TimeSpan yearOld = DateTime.Now - user.BirthDate.AddYears(18);
                if (yearOld.Days < 0)
                {
                    return null!;
                }
            }
            int upvotesCount = await _artworksRepository.GetArtworkUpvotesCount(id);
            int downvotesCount = await _artworksRepository.GetArtworkDownvotesCount(id);
            if (artwork is null)
            {
                return null!;
            }

            artwork.Views++;
            await _artworksRepository.SaveArtwork(artwork);

            return new ArtworkDetailsDTO()
            {
                ArtType = artwork!.ArtType,
                Id = artwork.Id,
                Owner = new UserDTO()
                {
                    Username = artwork.Owner!.UserName,
                    Id = artwork.Owner!.Id
                },
                resourceUrls = artwork.Files!
                    .Select(file => _cloudflareFileService.GetFileUrl(file.Key!))
                    .ToList()!,
                Tags = artwork.Tags!.Select(tag => tag.TagName)!,
                Genres = artwork.Genres!.Select(genre => genre.GenreName).ToList()!,
                Title = artwork.Title,
                Description = artwork.Description,
                Views = artwork.Views,
                Upvotes = upvotesCount,
                Downvotes = downvotesCount
            };
        }

        public async Task<List<ArtworkDTO>> GetPopularArtworks(ArtType artType)
        {
            List<Artwork> artworks = await _artworksRepository.GetPopularArtworks(artType);

            return artworks
                .Select(
                    artwork =>
                        new ArtworkDTO
                        {
                            ArtType = artwork.ArtType,
                            Genres = artwork.Genres!.Select(genre => genre.GenreName).ToList()!,
                            Id = artwork.Id,
                            Tags = artwork.Tags!.Select(tag => tag.TagName).ToList()!,
                            Title = artwork.Title,
                            Views = artwork.Views,
                            Upvotes = artwork.Upvotes!.Count,
                            Downvotes = artwork.Downvotes!.Count,
                            ThumbnailUrl =
                                artwork.Files!.Count == 0
                                    ? ""
                                    : _cloudflareFileService.GetFileUrl(artwork.Files!.First().Key!)
                        }
                )
                .ToList();
        }

        public async Task<List<ReportDTO>> GetReports()
        {
            List<Report> reports = await _artworksRepository.GetReports();

            return reports
                .Select(
                    report =>
                        new ReportDTO()
                        {
                            ArtworkId = report.Artwork!.Id,
                            ArtworkTitle = report.Artwork.Title,
                            CreatedAt = report.CreatedAt,
                            ReportId = report.Id,
                            ReportReason = report.ReportReason
                        }
                )
                .ToList();
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
                            Views = artwork.Views,
                            Upvotes = artwork.Upvotes!.Count,
                            Downvotes = artwork.Downvotes!.Count,
                            ThumbnailUrl =
                                artwork.Files!.Count() > 0
                                    ? _cloudflareFileService.GetFileUrl(artwork.Files!.First().Key!)
                                    : ""
                        }
                )
                .ToList();

            return artworkDTOs;
        }

        public async Task<StatsDTO> GetUserStats(string token)
        {
            User? user = await _accountRepository.GetUserByToken(token);

            List<StatsPerArtworkTypeDTO> artworksCountByArtType =
                await _artworksRepository.GetArtworksCountByArtType(user);

            List<StatsPerArtworkTypeDTO> artworksViewsByArtType =
                await _artworksRepository.GetArtworksViewsByArtType(user);

            List<StatsPerArtworkTypeDTO> artworksCommentsCountByArtType =
                await _artworksRepository.GetArtworksCommentsCountByArtType(user);

            VotesCountDTO votesCount = await _artworksRepository.GetArtworksVotes(user);

            return new StatsDTO()
            {
                ArtworksCount = artworksCountByArtType,
                ArtworksCommentsCount = artworksCommentsCountByArtType,
                ArtworksViewsCount = artworksViewsByArtType,
                Votes = votesCount
            };
        }

        public async Task<StatsDTO> GetUserStats(Guid userId)
        {
            User? user = await _accountRepository.GetUserById(userId);

            List<StatsPerArtworkTypeDTO> artworksCountByArtType =
                await _artworksRepository.GetArtworksCountByArtType(user);

            List<StatsPerArtworkTypeDTO> artworksViewsByArtType =
                await _artworksRepository.GetArtworksViewsByArtType(user);

            List<StatsPerArtworkTypeDTO> artworksCommentsCountByArtType =
                await _artworksRepository.GetArtworksCommentsCountByArtType(user);

            VotesCountDTO votesCount = await _artworksRepository.GetArtworksVotes(user);

            return new StatsDTO()
            {
                ArtworksCount = artworksCountByArtType,
                ArtworksCommentsCount = artworksCommentsCountByArtType,
                ArtworksViewsCount = artworksViewsByArtType,
                Votes = votesCount
            };
        }

        public async Task ReportArtwork(
            string token,
            Guid artworkId,
            ReportRequestDTO reportRequestDTO
        )
        {
            Artwork? artwork = await _artworksRepository.GetArtworkById(artworkId)!;
            User? user = await _accountRepository.GetUserByToken(token);
            Report report = new Report()
            {
                Artwork = artwork,
                CreatedAt = DateTime.Now,
                Id = Guid.NewGuid(),
                ReportedBy = user,
                ReportReason = reportRequestDTO.ReportReason
            };

            await _artworksRepository.AddReport(report);
        }

        public async Task<List<ArtworkDTO>> SearchArtworks(
            string? query,
            ArtType? artType,
            string? genre,
            string? tags
        )
        {
            List<string> tagsList = new List<string>();
            if (tags is not null)
            {
                tagsList = tags.Split(",").ToList();
            }
            List<ArtType?> artTypes = new List<ArtType?>();
            if (artType is not null)
            {
                artTypes.Add(artType!);
            }
            else
            {
                artTypes.Add(ArtType.MUSIC);
                artTypes.Add(ArtType.LITERATURE);
                artTypes.Add(ArtType.PHOTOGRAPHY);
                artTypes.Add(ArtType.OTHER);
            }
            List<Artwork> artworks = await _artworksRepository.SearchArtworks(
                query ?? "",
                artTypes,
                genre ?? "",
                tagsList
            );

            return artworks
                .Select(
                    artwork =>
                        new ArtworkDTO()
                        {
                            ArtType = artwork.ArtType,
                            Genres = artwork.Genres!.Select(genre => genre.GenreName).ToList()!,
                            Id = artwork.Id,
                            Tags = artwork.Tags!.Select(tag => tag.TagName).ToList()!,
                            Title = artwork.Title,
                            Views = artwork.Views,
                            Upvotes = artwork.Upvotes!.Count,
                            Downvotes = artwork.Downvotes!.Count,
                            ThumbnailUrl =
                                artwork.Files!.Count() > 0
                                    ? _cloudflareFileService.GetFileUrl(artwork.Files!.First().Key!)
                                    : ""
                        }
                )
                .ToList();
        }

        public async Task<bool> UpdateArtwork(
            string token,
            Guid id,
            UpdateArtworkRequestDTO updateArtworkRequestDTO
        )
        {
            User user = await _accountRepository.GetUserByToken(token);

            List<string>? userRoles = await _userManager.GetRolesAsync(user) as List<string>;

            Artwork artwork = await _artworksRepository.GetArtworkById(id);

            if ((user.Id != artwork.Owner!.Id) && !userRoles!.Contains("admin"))
            {
                return false;
            }

            artwork.Title = updateArtworkRequestDTO.Title;
            artwork.Description = updateArtworkRequestDTO.Description;
            artwork.ArtType = updateArtworkRequestDTO.ArtType;
            artwork.Genres = updateArtworkRequestDTO.Genres!
                .Select(genre => new Genre(genre))
                .ToList();
            artwork.Tags = updateArtworkRequestDTO.Tags!.Select(tag => new Tag(tag)).ToList();

            await _artworksRepository.SaveArtwork(artwork);

            return true;
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

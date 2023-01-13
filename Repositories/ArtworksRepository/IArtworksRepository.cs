using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_backend.Misc;

namespace praca_inzynierska_backend.Repositories.ArtworksRepository
{
    public interface IArtworksRepository
    {
        public Task<List<Artwork>> GetUserArtworks(Guid id);
        Task<Artwork> GetArtworkById(Guid id);
        Task<Artwork> GetArtworkWithUpvotesById(Guid id);
        Task<Artwork> GetArtworkWithDownVotesById(Guid id);
        Task<IEnumerable<Comment>> GetArtworkComments(Guid id);
        Task AddComment(Comment comment);
        Task DeleteArtwork(Artwork artwork);
        Task<bool> UpvoteArtwork(Upvote upvote);
        Task<int> GetArtworkUpvotesCount(Guid id);
        Task<int> GetArtworkDownvotesCount(Guid id);
        Task<bool> DownvoteArtwork(Downvote downvote);
        Task<Upvote> GetUpvote(Guid userId, Guid artworkId);
        Task DeleteUpvote(Upvote upvote);
        Task<Downvote> GetDownvote(Guid userId, Guid artworkId);
        Task DeleteDownvote(Downvote downvote);
        Task AddArtwork(Artwork artwork);
        Task SaveFile(Artwork artwork);
        Task SaveArtwork(Artwork artwork);
        Task<List<Artwork>> GetPopularArtworks(ArtType artType);
        Task<List<Report>> GetReports();
        Task AddReport(Report report);
        Task<Report> GetReportById(Guid reportId);
        Task DeleteReport(Report report);
        Task<List<Artwork>> SearchArtworks(
            string query,
            List<ArtType?> artTypes,
            string genre,
            List<string> tagsList
        );
        Task<List<StatsPerArtworkTypeDTO>> GetArtworksCountByArtType(User user);
        Task<List<StatsPerArtworkTypeDTO>> GetArtworksViewsByArtType(User user);
        Task<List<StatsPerArtworkTypeDTO>> GetArtworksCommentsCountByArtType(User user);
        Task<VotesCountDTO> GetArtworksVotes(User user);
    }
}

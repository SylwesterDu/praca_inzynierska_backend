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
        Task<Artwork>? GetArtworkById(Guid id);
        Task<Artwork> GetArtworkWithUpvotesById(Guid id);
        Task<Artwork> GetArtworkWithDownVotesById(Guid id);
        Task<IEnumerable<Review>> GetArtworkReviews(Guid id);
        Task AddReview(Review review);
        Task DeleteArtwork(Artwork artwork);
        Task<bool> VoteArtwork(Vote vote);
        Task<int> GetArtworkUpvotesCount(Guid id);
        Task<int> GetArtworkDownvotesCount(Guid id);
        Task DeleteVote(Vote vote);
        Task<Vote> GetVote(Guid userId, Guid artworkId);
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
        Task<List<StatsPerArtworkTypeDTO>> GetArtworksReviewsCountByArtType(User user);
        Task<VotesCountDTO> GetArtworksVotes(User user);
        Task SaveVote(Vote vote);
    }
}

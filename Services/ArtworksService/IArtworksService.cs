using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Misc;

namespace praca_inzynierska_backend.Services.ArtworksService
{
    public interface IArtworksService
    {
        public Task<List<ArtworkDTO>> GetUserArtworks(Guid id);
        Task<ArtworkDetailsDTO> GetArtworkDetails(Guid id, string? token);
        Task<List<ReviewDTO>> GetArtworkReviews(Guid id);
        Task AddReview(string token, Guid id, AddReviewDTO dto);
        Task<bool> DeleteArtwork(string token, Guid id);
        Task<bool> VoteArtwork(string token, Guid id, int value);
        Task<bool> UpdateArtwork(
            string token,
            Guid id,
            UpdateArtworkRequestDTO updateArtworkRequestDTO
        );
        Task<List<ArtworkDTO>> GetPopularArtworks(ArtType artType);
        Task<List<ReportDTO>> GetReports();
        Task ReportArtwork(string token, Guid artworkId, ReportRequestDTO reportRequestDTO);
        Task DeleteReport(Guid reportId);
        Task<List<ArtworkDTO>> SearchArtworks(
            string? query,
            ArtType? artType,
            string? genre,
            string? tags
        );
        Task<StatsDTO> GetUserStats(string token);
        Task<StatsDTO> GetUserStats(Guid userId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using praca_inzynierska_backend.Data;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_backend.Misc;

namespace praca_inzynierska_backend.Repositories.ArtworksRepository
{
    public class ArtworksRepository : IArtworksRepository
    {
        private AppDbContext _context;

        public ArtworksRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetArtworkReviews(Guid id)
        {
            Artwork? artwork = await _context.Artworks!.FirstOrDefaultAsync(
                artwork => artwork.Id == id
            );

            if (artwork is null)
            {
                return null!;
            }
            return await _context.Reviews!
                .Where(review => review.Artwork!.Id == id)
                .Include(review => review.Creator)
                .ThenInclude(creator => creator!.Avatar)
                .ToListAsync();
        }

        public async Task<Artwork> GetArtworkById(Guid id)
        {
            Artwork? artwork = await _context!.Artworks!
                .Where(_artwork => _artwork.Id == id)
                .Include(artwork => artwork.Owner)
                .Include(artwork => artwork.Tags)
                .Include(artwork => artwork.Genres)
                .Include(artwork => artwork.Files)
                .FirstOrDefaultAsync();

            return artwork!;
        }

        public async Task<List<Artwork>> GetUserArtworks(Guid id)
        {
            List<Artwork> artworks = await _context.Artworks!
                .Where(artwork => artwork.Owner!.Id == id && artwork.Published)
                .Include(artwork => artwork.Genres)
                .Include(artwork => artwork.Tags)
                .Include(artwork => artwork.Votes)
                .Include(artwork => artwork.Files)
                .ToListAsync();

            return artworks;
        }

        public async Task AddReview(Review review)
        {
            await _context.Reviews!.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArtwork(Artwork artwork)
        {
            _context.Artworks!.Remove(artwork);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> VoteArtwork(Vote vote)
        {
            await _context.Votes!.AddAsync(vote);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Artwork> GetArtworkWithUpvotesById(Guid id)
        {
            Artwork? artwork = await _context.Artworks!
                .Where(artwork => artwork.Id == id)
                .Include(artwork => artwork.Votes) //TODO:
                .FirstOrDefaultAsync();

            return artwork!;
        }

        public async Task<Artwork> GetArtworkWithDownVotesById(Guid id)
        {
            Artwork? artwork = await _context.Artworks!
                .Where(artwork => artwork.Id == id)
                .Include(artwork => artwork.Votes) //TODO:
                .FirstOrDefaultAsync();

            return artwork!;
        }

        public async Task<int> GetArtworkUpvotesCount(Guid id)
        {
            int count = await _context.Votes!
                .Where(vote => vote.Artwork!.Id == id && vote.Value == 1)
                .CountAsync();

            return count;
        }

        public async Task<int> GetArtworkDownvotesCount(Guid id)
        {
            int count = await _context.Votes!
                .Where(vote => vote.Artwork!.Id == id && vote.Value == -1)
                .CountAsync();

            return count;
        }

        public async Task<Vote> GetVote(Guid userId, Guid artworkId)
        {
            Vote? vote = await _context.Votes!.FirstOrDefaultAsync(
                d => d.Artwork!.Id == artworkId && d.User!.Id == userId
            );
            return vote!;
        }

        public async Task SaveVote(Vote vote)
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddArtwork(Artwork artwork)
        {
            await _context.Artworks!.AddAsync(artwork);
            await _context.SaveChangesAsync();
        }

        public async Task SaveFile(Artwork artwork)
        {
            await _context.SaveChangesAsync();
        }

        public async Task SaveArtwork(Artwork artwork)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Artwork>> GetPopularArtworks(ArtType artType)
        {
            DateTime weekAgo = DateTime.Now.Subtract(TimeSpan.FromDays(7));
            List<Artwork> popularMusic = await _context.Artworks!
                .Where(artwork => artwork.ArtType == artType && artwork.CreatedAt > weekAgo)
                .Include(artwork => artwork.Votes)
                .Include(artwork => artwork.Tags)
                .Include(artwork => artwork.Genres)
                .Include(artwork => artwork.Files)
                .OrderBy(artwork => artwork.Views)
                .Take(10)
                .ToListAsync();

            return popularMusic;
        }

        public async Task<List<Report>> GetReports()
        {
            List<Report> reports = await _context.Reports!
                .Include(report => report.Artwork)
                .Include(report => report.ReportedBy)
                .ToListAsync();

            return reports;
        }

        public async Task AddReport(Report report)
        {
            await _context.Reports!.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task<Report> GetReportById(Guid reportId)
        {
            Report report = await _context.Reports!.FirstAsync(report => report.Id == reportId);
            return report;
        }

        public async Task DeleteReport(Report report)
        {
            _context.Reports!.Remove(report);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Artwork>> SearchArtworks(
            string query,
            List<ArtType?> artTypes,
            string genre,
            List<string> tagsList
        )
        {
            List<Artwork> artworks;

            if (tagsList.Count() == 0)
            {
                artworks = await _context.Artworks!
                    .Include(artwork => artwork.Votes)
                    .Include(artwork => artwork.Tags)
                    .Include(artwork => artwork.Genres)
                    .Include(artwork => artwork.Files)
                    .OrderBy(artwork => artwork.Views)
                    .Where(
                        artwork =>
                            artwork.Title!.ToUpper().Contains(query!.ToUpper())
                            && artTypes.Contains((artwork.ArtType))
                            && artwork.Genres!.Any(
                                artworkGenre =>
                                    artworkGenre.GenreName!.ToUpper().Contains(genre.ToUpper())
                            )
                    )
                    .ToListAsync();
            }
            else
            {
                artworks = await _context.Artworks!
                    .Include(artwork => artwork.Votes)
                    .Include(artwork => artwork.Tags)
                    .Include(artwork => artwork.Genres)
                    .Include(artwork => artwork.Files)
                    .OrderBy(artwork => artwork.Views)
                    .Where(
                        artwork =>
                            artwork.Title!.ToUpper().Contains(query!.ToUpper())
                            && artTypes.Contains((artwork.ArtType))
                            && artwork.Genres!.Any(
                                artworkGenre =>
                                    artworkGenre.GenreName!.ToUpper().Contains(genre.ToUpper())
                            )
                            && artwork.Tags!.Any(
                                artworkTag => tagsList.Any(tag => tag == artworkTag.TagName)
                            )
                    )
                    .ToListAsync();
            }

            return artworks;
        }

        public async Task<List<StatsPerArtworkTypeDTO>> GetArtworksCountByArtType(User user)
        {
            List<StatsPerArtworkTypeDTO> stats = await _context.Artworks!
                .Where(artwork => artwork.Owner!.Id == user.Id && artwork.Published)
                .GroupBy(q => q.ArtType)
                .Select(
                    result =>
                        new StatsPerArtworkTypeDTO()
                        {
                            ArtType = result.Key,
                            Count = result.Count()
                        }
                )
                .ToListAsync();
            return stats;
        }

        public async Task<List<StatsPerArtworkTypeDTO>> GetArtworksViewsByArtType(User user)
        {
            List<StatsPerArtworkTypeDTO> stats = await _context.Artworks!
                .Where(artwork => artwork.Owner!.Id == user.Id && artwork.Published)
                .GroupBy(q => q.ArtType)
                .Select(
                    result =>
                        new StatsPerArtworkTypeDTO()
                        {
                            ArtType = result.Key,
                            Count = result.Sum(artwork => artwork.Views)
                        }
                )
                .ToListAsync();
            return stats;
        }

        public async Task<List<StatsPerArtworkTypeDTO>> GetArtworksReviewsCountByArtType(User user)
        {
            List<StatsPerArtworkTypeDTO> stats = await _context.Artworks!
                .Where(artwork => artwork.Owner!.Id == user.Id && artwork.Published)
                .GroupBy(q => q.ArtType)
                .Select(
                    result =>
                        new StatsPerArtworkTypeDTO()
                        {
                            ArtType = result.Key,
                            Count = result.Sum(artwork => artwork.Reviews!.Count())
                        }
                )
                .ToListAsync();
            return stats;
        }

        public async Task<VotesCountDTO> GetArtworksVotes(User user)
        {
            int upvotes = await _context.Votes!.CountAsync(
                vote => vote.Artwork!.Owner!.Id == user.Id && vote.Value == 1
            );
            int downvotes = await _context.Votes!.CountAsync(
                vote => vote.Artwork!.Owner!.Id == user.Id && vote.Value == -1
            );
            return new VotesCountDTO() { Upvotes = upvotes, Downvotes = downvotes };
        }

        public async Task DeleteVote(Vote vote)
        {
            _context.Remove(vote);
            await _context.SaveChangesAsync();
        }
    }
}

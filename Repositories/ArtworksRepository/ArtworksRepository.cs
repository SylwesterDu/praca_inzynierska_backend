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

        public async Task<IEnumerable<Comment>> GetArtworkComments(Guid id)
        {
            Artwork? artwork = await _context.Artworks!.FirstOrDefaultAsync(
                artwork => artwork.Id == id
            );

            if (artwork is null)
            {
                return null!;
            }
            return await _context.Comments!
                .Where(comment => comment.Artwork!.Id == id)
                .Include(comment => comment.Creator)
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
                .Include(artwork => artwork.Upvotes)
                .Include(artwork => artwork.Downvotes)
                .Include(artwork => artwork.Files)
                .ToListAsync();

            return artworks;
        }

        public async Task AddComment(Comment comment)
        {
            await _context.Comments!.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArtwork(Artwork artwork)
        {
            _context.Artworks!.Remove(artwork);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpvoteArtwork(Upvote upvote)
        {
            Upvote? _upvote = await _context.Upvotes!.FirstOrDefaultAsync(u => u.Equals(upvote));
            if (_upvote is not null)
            {
                return false;
            }

            await _context.Upvotes!.AddAsync(upvote!);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Artwork> GetArtworkWithUpvotesById(Guid id)
        {
            Artwork? artwork = await _context.Artworks!
                .Where(artwork => artwork.Id == id)
                .Include(artwork => artwork.Upvotes)
                .FirstOrDefaultAsync();

            return artwork!;
        }

        public async Task<Artwork> GetArtworkWithDownVotesById(Guid id)
        {
            Artwork? artwork = await _context.Artworks!
                .Where(artwork => artwork.Id == id)
                .Include(artwork => artwork.Upvotes)
                .FirstOrDefaultAsync();

            return artwork!;
        }

        public async Task<int> GetArtworkUpvotesCount(Guid id)
        {
            int count = await _context.Artworks!
                .Where(artwork => artwork.Id == id)
                .Select(artwork => artwork.Upvotes!.Count)
                .FirstOrDefaultAsync();

            return count;
        }

        public async Task<int> GetArtworkDownvotesCount(Guid id)
        {
            int count = await _context.Artworks!
                .Where(artwork => artwork.Id == id)
                .Select(artwork => artwork.Downvotes!.Count)
                .FirstOrDefaultAsync();

            return count;
        }

        public async Task<bool> DownvoteArtwork(Downvote downvote)
        {
            Downvote? _downvote = await _context.Downvotes!.FirstOrDefaultAsync(
                d => d.Equals(downvote)
            );
            if (_downvote is not null)
            {
                return false;
            }

            await _context.Downvotes!.AddAsync(downvote!);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Upvote> GetUpvote(Guid userId, Guid artworkId)
        {
            Upvote? upvote = await _context.Upvotes!.FirstOrDefaultAsync(
                u => u.Artwork!.Id == artworkId && u.User!.Id == userId
            );
            return upvote!;
        }

        public async Task DeleteUpvote(Upvote upvote)
        {
            _context.Upvotes!.Remove(upvote);
            await _context.SaveChangesAsync();
        }

        public async Task<Downvote> GetDownvote(Guid userId, Guid artworkId)
        {
            Downvote? downvote = await _context.Downvotes!.FirstOrDefaultAsync(
                d => d.Artwork!.Id == artworkId && d.User!.Id == userId
            );
            return downvote!;
        }

        public async Task DeleteDownvote(Downvote downvote)
        {
            _context.Downvotes!.Remove(downvote);
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
                .Include(artwork => artwork.Upvotes)
                .Include(artwork => artwork.Downvotes)
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
                    .Include(artwork => artwork.Upvotes)
                    .Include(artwork => artwork.Downvotes)
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
                    .Include(artwork => artwork.Upvotes)
                    .Include(artwork => artwork.Downvotes)
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
    }
}

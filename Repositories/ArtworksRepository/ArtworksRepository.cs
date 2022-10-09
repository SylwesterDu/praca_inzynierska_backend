using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data;
using praca_inzynierska_backend.Data.DTOs;
using Microsoft.EntityFrameworkCore;
using praca_inzynierska_praca_inzynierska_backend.Data.Entities;

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
            Artwork? artwork = await _context.Artworks!.FirstOrDefaultAsync(artwork => artwork.Id == id);

            if (artwork is null)
            {
                return null!;
            }
            return await _context.Comments!.Where(comment => comment.Artwork!.Id == id)
            .Include(comment => comment.Creator).ToListAsync();
        }

        public async Task<Artwork> GetArtworkById(Guid id)
        {
            Artwork? artwork = await _context!.Artworks!.Where(_artwork => _artwork.Id == id)
                .Include(artwork => artwork.Owner)
                .Include(artwork => artwork.Tags)
                .Include(artwork => artwork.Genres)
                .Include(artwork => artwork.FilesData)
                .FirstOrDefaultAsync();

            return artwork!;
        }

        public async Task<List<Artwork>> GetUserArtworks(Guid id)
        {
            User? user = await _context.Users!.Where(user => user.Id == id)
                .Include(user => user.Artworks)!
                .ThenInclude(artwork => artwork.Tags)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                return new List<Artwork>();
            }

            if (user.Artworks is null)
            {
                return new List<Artwork>();

            }

            List<Artwork> artworks = await _context.Artworks!.Where(artwork => artwork.Owner!.Id == id)
                .Include(artwork => artwork.Genres)
                .Include(artwork => artwork.Tags).ToListAsync();

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
    }
}
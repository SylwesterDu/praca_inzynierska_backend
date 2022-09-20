using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Data.DTOs;
using backend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.ArtworksRepository
{
    public class ArtworksRepository : IArtworksRepository
    {
        private AppDbContext _context;

        public ArtworksRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> getArtworkComments(Guid id)
        {
            Artwork? artwork = await _context.Artworks!.FirstOrDefaultAsync(artwork => artwork.Id == id);

            if (artwork is null)
            {
                return null!;
            }
            return await _context.Comments!.Where(comment => comment.Artwork!.Id == id)
            .Include(comment => comment.Creator).ToListAsync(); //TODO: sprawdz czy pobiera komentarze
        }

        public async Task<Artwork> getArtworkById(Guid id)
        {
            Artwork? artwork = await _context!.Artworks!.Where(_artwork => _artwork.Id == id)
                .Include(artwork => artwork.Owner)
                .Include(artwork => artwork.Tags).FirstOrDefaultAsync();

            return artwork!;
        }

        public async Task<UserArtworksResponseDTO> GetUserArtworks(Guid id)
        {
            User? user = await _context.Users!.Where(user => user.Id == id)
                .Include(user => user.Artworks)!
                .ThenInclude(artwork => artwork.Tags).FirstOrDefaultAsync();

            return new UserArtworksResponseDTO()
            {
                Artworks = user!.Artworks!.Select(artwork => new ArtworkDTO()
                {
                    Id = artwork.Id,
                    ArtType = artwork.ArtType,
                    DownVotes = artwork.DownVotes,
                    UpVotes = artwork.UpVotes,
                    Title = artwork.Title,
                    Tags = artwork!.Tags!.Select(tag => tag.TagName)!,
                    Views = artwork.Views
                }).ToList()
            };
        }

        public async Task addComment(Comment comment)
        {
            await _context.Comments!.AddAsync(comment);
            await _context.SaveChangesAsync();
        }
    }
}
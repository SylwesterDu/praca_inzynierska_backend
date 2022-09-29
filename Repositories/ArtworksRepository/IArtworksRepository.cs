using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_praca_inzynierska_backend.Data.Entities;

namespace praca_inzynierska_backend.Repositories.ArtworksRepository
{
    public interface IArtworksRepository
    {
        public Task<List<Artwork>> GetUserArtworks(Guid id);
        Task<Artwork> getArtworkById(Guid id);
        Task<IEnumerable<Comment>> getArtworkComments(Guid id);
        Task addComment(Comment comment);
    }
}
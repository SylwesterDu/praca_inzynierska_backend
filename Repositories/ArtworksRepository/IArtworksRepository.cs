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
        Task<Artwork> GetArtworkById(Guid id);
        Task<Artwork> GetArtworkWithUpvotesById(Guid id);
        Task<Artwork> GetArtworkWithDownVotesById(Guid id);
        Task<IEnumerable<Comment>> GetArtworkComments(Guid id);
        Task AddComment(Comment comment);
        Task DeleteArtwork(Artwork artwork);
        Task<bool> UpvoteArtwork(User user, Artwork artwork);
        Task<int> GetArtworkUpvotesCount(Guid id);
    }
}

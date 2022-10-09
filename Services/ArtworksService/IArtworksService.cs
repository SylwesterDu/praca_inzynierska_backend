using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;

namespace praca_inzynierska_backend.Services.ArtworksService
{
    public interface IArtworksService
    {
        public Task<List<ArtworkDTO>> GetUserArtworks(Guid id);
        Task<ArtworkDetailsDTO> GetArtworkDetails(Guid id);
        Task<IEnumerable<CommentDTO>> GetArtworkComments(Guid id);
        Task AddComment(string token, Guid id, string content);
        Task<bool> DeleteArtwork(string token, Guid id);
    }
}
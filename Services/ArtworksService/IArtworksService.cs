using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;

namespace praca_inzynierska_backend.Services.ArtworksService
{
    public interface IArtworksService
    {
        public Task<UserArtworksResponseDTO> GetUserArtworks(Guid id);
        Task<ArtworkDetailsDTO> getArtworkDetails(Guid id);
        Task<IEnumerable<CommentDTO>> getArtworkComments(Guid id);
        Task AddComment(string token, Guid id, string content);
    }
}
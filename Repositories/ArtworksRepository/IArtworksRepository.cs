using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.DTOs;
using backend.Data.Entities;

namespace backend.Repositories.ArtworksRepository
{
    public interface IArtworksRepository
    {
        public Task<UserArtworksResponseDTO> GetUserArtworks(Guid id);
        Task<Artwork> getArtworkById(Guid id);
        Task<IEnumerable<Comment>> getArtworkComments(Guid id);
        Task addComment(Comment comment);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.Entities;

namespace praca_inzynierska_backend.Repositories.AccountRepository
{
    public interface IAccountRepository
    {
        Task<User> GetUserByToken(string token);
        Guid GetUserIdFromToken(string token);
        Task<User> GetUserById(Guid id);
        Task<User> GetUserWithArtworksById(Guid id);
        Task<User> GetUserWithUpvotesById(Guid id);
        Task SaveChanges(User user);
        Task AddAvatar(AvatarFile avatarFile);
    }
}

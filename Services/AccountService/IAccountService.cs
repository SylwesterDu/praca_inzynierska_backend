using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Data.Entities;

namespace praca_inzynierska_backend.Services.AccountService
{
    public interface IAccountService
    {
        Task ChangeAvatar(string token, IFormFile file);
        public JwtSecurityToken GetToken(List<Claim> claims);
        public Task<User> GetUserByToken(string token);
        Task<UserDetailsDTO> GetUserDetails(Guid userId);
        Task<UserDTO> GetUserInfo(string token);
    }
}

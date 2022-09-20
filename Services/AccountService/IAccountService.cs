using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Data.Entities;

namespace backend.Services.AccountService
{
    public interface IAccountService
    {
        public JwtSecurityToken GetToken(List<Claim> claims);
        Task<User> getUserByToken(string token);
    }
}
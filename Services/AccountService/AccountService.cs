using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using backend.Data.DTOs;
using backend.Data.Entities;
using backend.Repositories.AccountRepository;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        public AccountService(IConfiguration configuration, IAccountRepository repository)
        {
            _configuration = configuration;
            _accountRepository = repository;
        }

        public JwtSecurityToken GetToken(List<Claim> claims)
        {

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(30),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;

        }

        public async Task<User> getUserByToken(string token)
        {
            User user = await _accountRepository.getUserByToken(token);
            return user;
        }
    }
}
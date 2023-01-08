using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_backend.Repositories.AccountRepository;

namespace praca_inzynierska_backend.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<User> _userManager;

        public AccountService(
            IConfiguration configuration,
            IAccountRepository repository,
            UserManager<User> userManager
        )
        {
            _configuration = configuration;
            _accountRepository = repository;
            _userManager = userManager;
        }

        public JwtSecurityToken GetToken(List<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(30),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    authSigningKey,
                    SecurityAlgorithms.HmacSha256
                )
            );

            return token;
        }

        public async Task<User> GetUserByToken(string token)
        {
            User user = await _accountRepository.GetUserByToken(token);

            return user;
        }

        public async Task<UserDTO> GetUserInfo(string token)
        {
            User user = await GetUserByToken(token);
            List<string>? roles = await _userManager.GetRolesAsync(user!) as List<string>;

            return new UserDTO()
            {
                Id = user.Id,
                Username = user.UserName,
                Roles = roles!.ToArray()
            };
        }
    }
}

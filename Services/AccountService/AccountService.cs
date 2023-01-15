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
using praca_inzynierska_backend.Repositories.ArtworksRepository;
using praca_inzynierska_backend.Services.CloudflareFileService;

namespace praca_inzynierska_backend.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        private readonly IArtworksRepository _artworksRepository;
        private readonly ICloudflareFileService _cloudflareFileService;

        public AccountService(
            IConfiguration configuration,
            IAccountRepository repository,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IArtworksRepository artworksRepository,
            ICloudflareFileService cloudflareFileService
        )
        {
            _configuration = configuration;
            _accountRepository = repository;
            _userManager = userManager;
            _roleManager = roleManager;
            _artworksRepository = artworksRepository;
            _cloudflareFileService = cloudflareFileService;
        }

        public async Task ChangeAvatar(string token, IFormFile file)
        {
            User user = await _accountRepository.GetUserByToken(token);
            string key = await _cloudflareFileService.UploadAvatar(user.Id, file);
            AvatarFile avatarFile = new AvatarFile()
            {
                Id = Guid.NewGuid(),
                key = key,
                user = user
            };

            // user.Avatar = avatarFile;

            // await _accountRepository.SaveChanges(user);
            await _accountRepository.AddAvatar(avatarFile);
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

        public async Task<UserDetailsDTO> GetUserDetails(Guid userId)
        {
            User user = await _accountRepository.GetUserById(userId);
            List<StatsPerArtworkTypeDTO> stats =
                await _artworksRepository.GetArtworksCountByArtType(user);

            return new UserDetailsDTO()
            {
                Id = userId,
                Username = user.UserName,
                Stats = stats,
                Avatar = user.Avatar is null
                    ? null
                    : _cloudflareFileService.GetFileUrl(user.Avatar.key!)
            };
        }

        public async Task<UserDTO> GetUserInfo(string token)
        {
            User user = await GetUserByToken(token);
            List<string>? roles = await _userManager.GetRolesAsync(user!) as List<string>;

            return new UserDTO()
            {
                Id = user.Id,
                Username = user.UserName,
                Roles = roles!.ToArray(),
                Avatar = user.Avatar is null
                    ? null
                    : _cloudflareFileService.GetFileUrl(user.Avatar.key!)
            };
        }

        public async Task<bool> Register(RegisterRequestDTO registerRequestDTO)
        {
            User? user = await _userManager.FindByEmailAsync(registerRequestDTO.Email);
            if (user is not null)
            {
                return false;
            }

            PasswordValidator<User> passwordValidator = new();
            IdentityResult result = await passwordValidator.ValidateAsync(
                _userManager,
                null!,
                registerRequestDTO.Password
            );
            if (!result.Succeeded)
            {
                return false;
            }

            User newUser =
                new()
                {
                    Id = new Guid(),
                    UserName = registerRequestDTO.Username,
                    Email = registerRequestDTO.Email,
                };

            result = await _userManager.CreateAsync(newUser, registerRequestDTO.Password);

            if (!result.Succeeded)
            {
                return false;
            }

            if (!(await _roleManager.RoleExistsAsync("standard_user")))
            {
                await _roleManager.CreateAsync(new Role("standard_user"));
            }

            if (!(await _roleManager.RoleExistsAsync("admin")))
            {
                await _roleManager.CreateAsync(new Role("admin"));
            }

            if (registerRequestDTO.Email!.Split("@").Last() == "admin.pl")
            {
                await _userManager.AddToRoleAsync(newUser, "admin");
            }

            var roleResult = await _userManager.AddToRoleAsync(newUser, "standard_user");
            if (!roleResult.Succeeded)
            {
                return false;
            }

            return true;
        }
    }
}

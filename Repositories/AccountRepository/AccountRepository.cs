using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using praca_inzynierska_backend.Data;
using praca_inzynierska_backend.Data.Entities;

namespace praca_inzynierska_backend.Repositories.AccountRepository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<User> GetUserById(Guid id)
        {
            User? user = await _context.Users!.FirstOrDefaultAsync(user => user.Id == id);
            return user!;
        }

        public async Task<User> GetUserByToken(string token)
        {
            Guid userId = parseJwtToken(token).Id;

            User? user = await _context.Users!.FirstOrDefaultAsync(user => user.Id == userId);
            return user!;
        }

        public Guid GetUserIdFromToken(string token)
        {
            Guid id = parseJwtToken(token).Id;
            return id;
        }

        public async Task<User> GetUserWithArtworksById(Guid id)
        {
            User? user = await _context.Users!
                .Where(user => user.Id == id)
                .Include(user => user.Artworks)
                .FirstOrDefaultAsync();

            return user!;
        }

        public async Task<User> GetUserWithUpvotesById(Guid id)
        {
            User? user = await _context.Users!
                .Where(user => user.Id == id)
                .Include(user => user.Upvotes)
                .FirstOrDefaultAsync();

            return user!;
        }

        private ParsedJwtToken parseJwtToken(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken? securityToken = handler.ReadToken(token) as JwtSecurityToken;

            string id = securityToken!.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            var roles = securityToken.Claims
                .Where(claim => claim.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();

            return new ParsedJwtToken() { Id = new Guid(id), Roles = roles };
        }
    }

    public class ParsedJwtToken
    {
        public Guid Id { get; set; }
        public List<string>? Roles { get; set; }
    }
}

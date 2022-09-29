using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data;
using Microsoft.EntityFrameworkCore;
using praca_inzynierska_praca_inzynierska_backend.Data.Entities;

namespace praca_inzynierska_backend.Repositories.AccountRepository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<User> getUserByToken(string token)
        {
            Guid userId = parseJwtToken(token).Id;

            User? user = await _context.Users!.FirstOrDefaultAsync(user => user.Id == userId);
            return user!;
        }

        protected ParsedJwtToken parseJwtToken(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken? securityToken = handler.ReadToken(token) as JwtSecurityToken;


            string id = securityToken!.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            var roles = securityToken.Claims.Where(claim => claim.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();

            return new ParsedJwtToken()
            {
                Id = new Guid(id),
                Roles = roles
            };
        }
    }

    public class ParsedJwtToken
    {
        public Guid Id { get; set; }
        public List<string>? Roles { get; set; }
    }
}
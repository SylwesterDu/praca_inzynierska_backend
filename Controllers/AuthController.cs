using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_backend.Services.AccountService;

namespace praca_inzynierska_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAccountService _accountService;
        private readonly IAccountService _service;

        public AuthController(
            UserManager<User> userManager,
            IAccountService service,
            IAccountService accountService
        )
        {
            _userManager = userManager;
            _service = service;
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO registerRequestDTO)
        {
            bool success = await _accountService.Register(registerRequestDTO);

            return Ok("User created!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            User user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);
            if (user is null)
            {
                return NotFound("User with given email doesn't exists!");
            }

            bool success = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (!success)
            {
                return Conflict("Check your password and try again!");
            }

            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _service.GetToken(authClaims);

            return Ok(new { Jwt = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [Authorize]
        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount()
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string token = values[0].Split(' ')[1];
            User user = await _service.GetUserByToken(token);
            await _userManager.DeleteAsync(user);
            return Ok();
        }
    }
}

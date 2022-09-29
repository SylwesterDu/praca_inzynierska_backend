using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Services.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using praca_inzynierska_praca_inzynierska_backend.Data.Entities;

namespace praca_inzynierska_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IAccountService _service;

        public AuthController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IAccountService service
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO registerRequestDTO)
        {
            if (registerRequestDTO.Password != registerRequestDTO.ConfirmPassword)
            {
                return Conflict("Check your passoword!");
            }
            User user = await _userManager.FindByNameAsync(registerRequestDTO.Username);
            if (user is not null)
            {
                return Conflict("Username is already taken!");
            }

            user = await _userManager.FindByEmailAsync(registerRequestDTO.Email);
            if (user is not null)
            {
                return Conflict("Email is already used!");
            }

            User newUser = new()
            {
                Id = new Guid(),
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Email,

            };

            IdentityResult result = await _userManager.CreateAsync(newUser, registerRequestDTO.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong. Try again" });
            }


            if (!(await _roleManager.RoleExistsAsync("standard_user")))
            {
                await _roleManager.CreateAsync(new Role("standard_user"));
            }

            var roleResult = await _userManager.AddToRoleAsync(newUser, "standard_user");
            if (!roleResult.Succeeded)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong. Try again2" });
            }

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



            return Ok(new
            {
                Jwt = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [Authorize]
        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount()
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string token = values[0].Split(' ')[1];
            User user = await _service.getUserByToken(token);
            await _userManager.DeleteAsync(user);
            return Ok();
        }
    }
}
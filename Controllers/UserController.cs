using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Data.Entities;
using praca_inzynierska_backend.Services.AccountService;
using praca_inzynierska_backend.Services.ArtworksService;

namespace praca_inzynierska_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IArtworksService _artworksService;
        private UserManager<User> _userManager;
        private IAccountService _accountService;

        public UserController(IArtworksService artworksService, UserManager<User> userManager, IAccountService accountService)
        {
            _artworksService = artworksService;
            _userManager = userManager;
            _accountService = accountService;
        }

        [HttpGet("{id}/artworks")]
        public async Task<IActionResult> GetUserArtworks(Guid id)
        {
            User user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                return NotFound("User with given id does not exists!");
            }
            List<ArtworkDTO> artworks = await _artworksService.GetUserArtworks(id);
            return Ok(artworks);
        }

        [Authorize]
        [HttpGet("artworks")]
        public async Task<IActionResult> GetArtworks()
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string token = values[0].Split(' ')[1];

            User user = await _accountService.GetUserByToken(token);

            List<ArtworkDTO> artworks = await _artworksService.GetUserArtworks(user.Id);
            return Ok(artworks);
        }

        [Authorize]
        [HttpGet()]
        public async Task<ActionResult<UserDTO>> GetUserInfo()
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string token = values[0].Split(' ')[1];

            UserDTO userDTO = await _accountService.GetUserInfo(token);

            return Ok(userDTO);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.DTOs;
using backend.Data.Entities;
using backend.Services.ArtworksService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IArtworksService _artworksService;
        private UserManager<User> _userManager;

        public UserController(IArtworksService artworksService, UserManager<User> userManager)
        {
            _artworksService = artworksService;
            _userManager = userManager;
        }

        [HttpGet("{id}/artworks")]
        public async Task<IActionResult> GetUserArtworks(Guid id)
        {
            User user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                return NotFound("User with given id does not exists!");
            }
            UserArtworksResponseDTO dto = await _artworksService.GetUserArtworks(id);
            return Ok(dto);
        }
    }
}
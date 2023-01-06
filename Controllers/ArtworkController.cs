using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Services.ArtworksService;

namespace praca_inzynierska_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtworkController : ControllerBase
    {
        private IArtworksService _artworksService;

        public ArtworkController(IArtworksService artworksService)
        {
            _artworksService = artworksService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtworkDetails(Guid id)
        {
            ArtworkDetailsDTO dto = await _artworksService.GetArtworkDetails(id);
            if (dto is null)
            {
                return NotFound("Artwork with given id does not exists!");
            }
            return Ok(dto);
        }

        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetArtworkComments(Guid id)
        {
            IEnumerable<CommentDTO> dto = await _artworksService.GetArtworkComments(id);
            if (dto is null)
            {
                return NotFound("Artwork with given id does not exists!");
            }
            return Ok(dto);
        }

        [Authorize]
        [HttpPost("{id}/comment")]
        public async Task<IActionResult> AddComment(
            [FromRoute] Guid id,
            [FromBody] AddCommentDTO dto
        )
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string token = values[0].Split(' ')[1];

            if (dto.content!.Length == 0)
            {
                return BadRequest("Comment is too short!");
            }
            await _artworksService.AddComment(token, id, dto.content!);

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtwork(Guid id)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string token = values[0].Split(' ')[1];

            bool success = await _artworksService.DeleteArtwork(token, id);

            if (!success)
            {
                return Conflict("owner and logged user aren't the same person!");
            }

            return Ok();
        }

        [Authorize]
        [HttpPut("{id}/upvote")]
        public async Task<IActionResult> Upvote(Guid id)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string token = values[0].Split(' ')[1];

            bool success = await _artworksService.UpvoteArtwork(token, id);

            if (!success)
            {
                return Conflict("You already upvoted this artwork!");
            }

            return Ok();
        }

        [Authorize]
        [HttpPut("{id}/downvote")]
        public async Task<IActionResult> Downvote(Guid id)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string token = values[0].Split(' ')[1];

            bool success = await _artworksService.DownvoteArtwork(token, id);

            if (!success)
            {
                return Conflict("You already downvoted this artwork!");
            }

            return Ok();
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateArtwork(
            [FromRoute] Guid id,
            [FromBody] UpdateArtworkRequestDTO updateArtworkRequestDTO
        )
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string token = values[0].Split(' ')[1];

            bool success = await _artworksService.UpdateArtwork(token, id, updateArtworkRequestDTO);

            if (!success)
            {
                return Conflict("You are not owner!");
            }

            return Ok();
        }
    }
}

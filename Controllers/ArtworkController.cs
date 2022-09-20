using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.DTOs;
using backend.Services.ArtworksService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace backend.Controllers
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
            ArtworkDetailsDTO dto = await _artworksService.getArtworkDetails(id);
            if (dto is null)
            {
                return NotFound("Artwork with given id does not exists!");
            }
            return Ok(dto);
        }

        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetArtworkComments(Guid id)
        {
            IEnumerable<CommentDTO> dto = await _artworksService.getArtworkComments(id);
            if (dto is null)
            {
                return NotFound("Artwork with given id does not exists!");
            }
            return Ok(dto);
        }

        [Authorize]
        [HttpPost("{id}/comment")]
        public async Task<IActionResult> AddComment([FromRoute] Guid id, [FromBody] AddCommentDTO dto)
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
    }
}
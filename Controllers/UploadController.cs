using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using praca_inzynierska_backend.Data.DTOs;
using praca_inzynierska_backend.Services.AccountService;
using praca_inzynierska_backend.Services.UploadService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace praca_inzynierska_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;

        public UploadController(IUploadService service)
        {
            _uploadService = service;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Begin()
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string token = values[0].Split(' ')[1];
            UploadProcessDTO processDTO = await _uploadService.CreateUploadProcess(token);

            return Ok(processDTO);
        }

        [Authorize]
        [HttpPost("{id}")]
        public async Task<IActionResult> UploadFile(IFormFile formFile, [FromRoute] Guid id)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string token = values[0].Split(' ')[1];

            bool result = await _uploadService.UploadFile(token, formFile, id);

            return Ok();
        }

        [Authorize]
        [HttpPost("{id}/publish")]
        public async Task<IActionResult> Publish([FromRoute] Guid id, [FromBody] PublishArtworkRequestDTO publishArtworkRequestDTO)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string token = values[0].Split(' ')[1];
            bool success = await _uploadService.PublishArtWork(token, id, publishArtworkRequestDTO);

            return Ok();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.DTOs;
using backend.Data.Entities;
using backend.Services.AccountService;
using backend.Services.UploadService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace backend.Controllers
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
        [HttpPost("upload-song")]
        public async Task<IActionResult> UploadSong(UploadSongRequestDTO uploadSongRequestDTO)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values);
            string token = values[0].Split(' ')[1];
            bool success = await _uploadService.UploadSong(token, uploadSongRequestDTO);

            if (success)
            {
                return Ok();

            }
            else
            {
                return Conflict("Song with given title already exists!");
            }
        }
    }
}
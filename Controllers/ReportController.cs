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
    public class ReportController : ControllerBase
    {
        private readonly IArtworksService _artworksService;

        public ReportController(IArtworksService artworksService)
        {
            _artworksService = artworksService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetReports()
        {
            List<ReportDTO> reports = await _artworksService.GetReports();
            return Ok(reports);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{reportId}")]
        public async Task<IActionResult> GetReports(Guid reportId)
        {
            await _artworksService.DeleteReport(reportId);
            return Ok();
        }
    }
}

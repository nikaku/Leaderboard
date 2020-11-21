using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leaderboard.Services.ImportExport;
using Leaderboard.Services.LeaderboardServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private ILeaderboardService _leaderboardService;
        private IImportExportService _importExportService;

        public ExportController(ILeaderboardService leaderboardService, IImportExportService importExportService)
        {
            _leaderboardService = leaderboardService;
            _importExportService = importExportService;
        }

        [HttpGet]
        public IActionResult DownloadLeaderboardByDay(DateTime date)
        {
            var leaderboard = _leaderboardService.GetLeaderboardByDay(date);
            var xz = _importExportService.ExportToExcel(leaderboard);
            return Ok();
        }
    }
}

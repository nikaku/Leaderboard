using Leaderboard.BL.Dtos.LeaderboardDto;
using Leaderboard.Services.ImportExport;
using Leaderboard.Services.LeaderboardServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Leaderboard.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private ILeaderboardService _leaderboardService;
        private IImportExportService _importExportService;
        private string _path;

        public ExportController(ILeaderboardService leaderboardService, IImportExportService importExportService)
        {
            _leaderboardService = leaderboardService;
            _importExportService = importExportService;

            _path = Path.Combine(Path.GetTempPath(), "ExportedExcelFiles");
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
        }

        [HttpGet]
        public IActionResult DownloadLeaderboardByDay(DateTime date)
        {
            string filePath = Path.Combine(_path, $"LeaderboardByDay-{DateTime.Now:yyyy-dd-MM}.xlsx");

            var leaderboard = _leaderboardService.GetLeaderboardByDay(date, true);
            if (leaderboard == null)
            {
                return NotFound();
            }

            _importExportService.ExportToExcel(leaderboard, filePath);
            return Ok(filePath);
        }

        [HttpGet]
        public IActionResult DownloadLeaderboardByWeek(DateTime date)
        {
            string filePath = Path.Combine(_path, $"LeaderboardByWeek-{DateTime.Now:yyyy-dd-MM}.xlsx");

            var leaderboard = _leaderboardService.GetLeaderboardByWeek(date, true);
            if (leaderboard == null)
            {
                return NotFound();
            }

            _importExportService.ExportToExcel(leaderboard, filePath);
            return Ok(filePath);
        }

        [HttpGet]
        public IActionResult DownloadLeaderboardByMonth(DateTime date)
        {
            string filePath = Path.Combine(_path, $"LeaderboardByMonth-{DateTime.Now:yyyy-dd-MM}.xlsx");

            var leaderboard = _leaderboardService.GetLeaderboardByWeek(date, true);
            if (leaderboard == null)
            {
                return NotFound();
            }

            _importExportService.ExportToExcel(leaderboard, filePath);
            return Ok(filePath);
        }


        [HttpGet]
        public IActionResult DownloadAllData()
        {
            string filePath = Path.Combine(_path, $"DownloadAllData-{DateTime.Now:yyyy-dd-MM}.xlsx");

            var leaderboard = _leaderboardService.GetAllData(true);
            if (leaderboard == null)
            {
                return NotFound();
            }

            _importExportService.ExportToExcel(leaderboard, filePath);
            return Ok(filePath);
        }

        [HttpGet]
        public IActionResult DownloadStats()
        {
            string filePath = Path.Combine(_path, $"DownloadStats-{DateTime.Now:yyyy-dd-MM}.xlsx");

            var leaderboard = _leaderboardService.GetStats(true);
            if (leaderboard == null)
            {
                return NotFound();
            }

            _importExportService.ExportToExcel<Stats>(leaderboard, filePath);
            return Ok(filePath);
        }
    }
}

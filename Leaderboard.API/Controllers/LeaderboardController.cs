using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leaderboard.BL.Interfaces;
using Leaderboard.Services.LeaderboardServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(IUnitOfWork unitOfWork, ILeaderboardService leaderboardService)
        {
            _unitOfWork = unitOfWork;
            _leaderboardService = leaderboardService;
        }

        [HttpGet]
        public IActionResult GetLeaderboardByDay(DateTime day)
        {
            var leaderboard = _leaderboardService.GetLeaderboardByDay(day);
            return Ok(leaderboard);
        }

        [HttpGet]
        public IActionResult GetLeaderboardByWeek(DateTime date)
        {
            var leaderboard = _leaderboardService.GetLeaderboardByWeek(date);
            return Ok(leaderboard);
        }

        [HttpGet]
        public IActionResult GetLeaderboardByMonth(DateTime date)
        {
            var leaderboard = _leaderboardService.GetLeaderboardByMonth(date);
            return Ok(leaderboard);
        }

        [HttpGet]
        public IActionResult GetAllData()
        {
            var leaderboard = _leaderboardService.GetAllData();
            return Ok(leaderboard);
        }

        [HttpGet]
        public IActionResult GetStats()
        {
            var stats = _leaderboardService.GetStats();
            return Ok(stats);
        }
    }
}

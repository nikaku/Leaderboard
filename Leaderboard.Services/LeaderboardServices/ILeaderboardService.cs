using Leaderboard.BL.Dtos.LeaderboardDto;
using Leaderboard.BL.Dtos.UserDtos;
using Leaderboard.BL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.Services.LeaderboardServices
{
    public interface ILeaderboardService
    {
        IEnumerable<LeaderboardDto> GetLeaderboardByDay(DateTime day);
        IEnumerable<LeaderboardDto> GetLeaderboardByWeek(DateTime week);
        IEnumerable<LeaderboardDto> GetLeaderboardByMonth(DateTime month);
        IEnumerable<LeaderboardDto> GetAllData();
        Stats GetStats();
        UserRating GetUserInfo(string username);
    }
}

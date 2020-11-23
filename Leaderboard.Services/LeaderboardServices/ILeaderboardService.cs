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
        IEnumerable<LeaderboardDto> GetLeaderboardByDay(DateTime day, bool ignoreCache = false);
        IEnumerable<LeaderboardDto> GetLeaderboardByWeek(DateTime week, bool ignoreCache = false);
        IEnumerable<LeaderboardDto> GetLeaderboardByMonth(DateTime month, bool ignoreCache = false);
        IEnumerable<LeaderboardDto> GetAllData(bool ignoreCache = false);
        Stats GetStats(bool ignoreCache = false);
        UserRating GetUserInfo(string username);
    }
}

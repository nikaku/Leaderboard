using Leaderboard.BL.Dtos.LeaderboardDto;
using System;
using System.Collections.Generic;

namespace Leaderboard.BL.Interfaces.Repositories
{
    public interface IUserScoreRepository
    {
        IEnumerable<LeaderboardDto> GetScoresByDay(DateTime day);
        IEnumerable<LeaderboardDto> GetScoresByWeek(DateTime week);
        IEnumerable<LeaderboardDto> GetScoresByMonth(DateTime month);
        IEnumerable<LeaderboardDto> GetAllData();
        Stats GetStats();
        
    }
}

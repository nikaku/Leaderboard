using Leaderboard.BL.Dtos.LeaderboardDto;
using Leaderboard.BL.Entities;
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
        int GetDalyAvarage();
        int GetWeeklyAvarage();
        int GetMonthlyAvarage();
        int GetDaiyMax();
        int GetWeeklyMax();
        int GetMonthlyMax();
        void AddOrUpdate(LeaderboardDto leaderboardDto, DateTime scoreDate);
    }
}

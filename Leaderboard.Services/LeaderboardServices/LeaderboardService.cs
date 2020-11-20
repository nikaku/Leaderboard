using Leaderboard.BL.Dtos.LeaderboardDto;
using Leaderboard.BL.Entities;
using Leaderboard.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaderboard.Services.LeaderboardServices
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaderboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<LeaderboardDto> GetLeaderboardByDay(DateTime day)
        {
            var dailyScores = _unitOfWork.UserScoreRepository.GetScoresByDay(day);
            return dailyScores.OrderByDescending(s => s.Score);
        }

        public IEnumerable<LeaderboardDto> GetLeaderboardByWeek(DateTime week)
        {
            var weeklyScores = _unitOfWork.UserScoreRepository.GetScoresByWeek(week);
            weeklyScores = weeklyScores.GroupBy(u => u.Username).Select(x =>
            new LeaderboardDto
            {
                Username = x.First().Username,
                Score = x.Sum(x => x.Score)
            });

            return weeklyScores.OrderByDescending(s => s.Score);
        }
        public IEnumerable<LeaderboardDto> GetLeaderboardByMonth(DateTime month)
        {
            var monthlyScores = _unitOfWork.UserScoreRepository.GetScoresByMonth(month);
            monthlyScores = monthlyScores.GroupBy(u => u.Username).Select(x =>
            new LeaderboardDto
            {
                Username = x.First().Username,
                Score = x.Sum(x => x.Score)
            });
            return monthlyScores;
        }

        public IEnumerable<LeaderboardDto> GetAllData()
        {
            return _unitOfWork.UserScoreRepository.GetAllData();
        }

        public Stats GetStats()
        {
           return _unitOfWork.UserScoreRepository.GetStats();
        }
    }
}

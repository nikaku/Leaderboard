using Leaderboard.BL.Caching;
using Leaderboard.BL.Dtos.LeaderboardDto;
using Leaderboard.BL.Dtos.UserDtos;
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
        private IStaticCacheManager _redisCacheManager;

        public LeaderboardService(IUnitOfWork unitOfWork, IStaticCacheManager staticCacheManager)
        {
            _unitOfWork = unitOfWork;
            _redisCacheManager = staticCacheManager;
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

            return monthlyScores.OrderByDescending(s => s.Score);
        }

        public IEnumerable<LeaderboardDto> GetAllData()
        {
            return _unitOfWork.UserScoreRepository.GetAllData();
        }

        public Stats GetStats()
        {
            CacheKey cacheKey = new CacheKey("GetStats");
            var statsInCache = _redisCacheManager.Get<Stats>(cacheKey);
            if (statsInCache != null)
            {
                return statsInCache;
            }

            var dailyAvg = _unitOfWork.UserScoreRepository.GetDalyAvarage();
            var weeklyAvg = _unitOfWork.UserScoreRepository.GetWeeklyAvarage();
            var monthlyAvg = _unitOfWork.UserScoreRepository.GetMonthlyAvarage();
            var dailyMax = _unitOfWork.UserScoreRepository.GetDalyMax();
            var weeklyMax = _unitOfWork.UserScoreRepository.GetWeeklyMax();
            var monthlyMax = _unitOfWork.UserScoreRepository.GetMonthlyMax();

            return new Stats
            {
                DailyAvarage = dailyAvg,
                WeeklyAvarage = weeklyAvg,
                MonthlyAvarage = monthlyAvg,
                DailyMax = dailyMax,
                WeeklyMax = weeklyMax,
                MonthlyMax = monthlyMax
            };
        }

        public UserRating GetUserInfo(string username)
        {
            var currentMonthScores = _unitOfWork.UserScoreRepository
                .GetScoresByMonth(DateTime.Now)
                .GroupBy(u => u.Username).Select(x =>
                     new LeaderboardDto
                     {
                         Username = x.First().Username,
                         Score = x.Sum(x => x.Score)
                     }).OrderByDescending(s => s.Score);

            if (currentMonthScores.Count() == 0)
            {
                throw new Exception("Scores Doesnot Exists");
            }

            var userRating = currentMonthScores.Select((Value, Index) => new { Value, Index }).SingleOrDefault(p => p.Value.Username == username);

            if (userRating == null)
            {
                throw new Exception("User Dont Have Scores");
            }

            return new UserRating
            {
                Username = userRating.Value.Username,
                Rating = userRating.Index + 1,
                Score = userRating.Value.Score
            };
        }
    }
}

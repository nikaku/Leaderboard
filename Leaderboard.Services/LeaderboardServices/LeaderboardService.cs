using Leaderboard.BL.Caching;
using Leaderboard.BL.Dtos.LeaderboardDto;
using Leaderboard.BL.Dtos.UserDtos;
using Leaderboard.BL.Interfaces;
using Leaderboard.BL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Leaderboard.Services.LeaderboardServices
{
    public class LeaderboardService : ILeaderboardService
    {
        private IStaticCacheManager _redisCacheManager;
        private IUserScoreRepository _userScoreRepository;

        public LeaderboardService(IUserScoreRepository userScoreRepository, IStaticCacheManager staticCacheManager)
        {
            _userScoreRepository = userScoreRepository;
            _redisCacheManager = staticCacheManager;
        }

        public IEnumerable<LeaderboardDto> GetLeaderboardByDay(DateTime day)
        {
            CacheKey cacheKey = new CacheKey(CacheKeys.LeaderboardByDayCacheKey);
            var LeaderboardByDay = _redisCacheManager.Get<IEnumerable<LeaderboardDto>>(cacheKey);

            if (LeaderboardByDay != null)
            {
                return LeaderboardByDay;
            }

            var dailyScores = _userScoreRepository.GetScoresByDay(day);
            var dailyScoresOrdered = dailyScores.OrderByDescending(s => s.Score);
            _redisCacheManager.Set(cacheKey, dailyScoresOrdered);

            return dailyScoresOrdered;
        }

        public IEnumerable<LeaderboardDto> GetLeaderboardByWeek(DateTime week)
        {
            CacheKey cacheKey = new CacheKey(CacheKeys.LeaderboardByWeekCacheKey);

            var LeaderboardByWeek = _redisCacheManager.Get<IEnumerable<LeaderboardDto>>(cacheKey);

            if (LeaderboardByWeek != null)
            {
                return LeaderboardByWeek;
            }

            var weeklyScores = _userScoreRepository.GetScoresByWeek(week);
            weeklyScores = weeklyScores.GroupBy(u => u.Username).Select(x =>
            new LeaderboardDto
            {
                Username = x.First().Username,
                Score = x.Sum(x => x.Score)
            });

            var weeklyScoresOrdered = weeklyScores.OrderByDescending(s => s.Score);
            _redisCacheManager.Set(cacheKey, weeklyScoresOrdered);

            return weeklyScoresOrdered;
        }

        public IEnumerable<LeaderboardDto> GetLeaderboardByMonth(DateTime month)
        {
            CacheKey cacheKey = new CacheKey(CacheKeys.LeaderboardByMonthCacheKey);

            var LeaderboardByMonth = _redisCacheManager.Get<IEnumerable<LeaderboardDto>>(cacheKey);

            if (LeaderboardByMonth != null)
            {
                return LeaderboardByMonth;
            }

            var monthlyScores = _userScoreRepository.GetScoresByMonth(month);
            monthlyScores = monthlyScores.GroupBy(u => u.Username).Select(x =>
            new LeaderboardDto
            {
                Username = x.First().Username,
                Score = x.Sum(x => x.Score)
            });

            var monthlyScoresOrdered = monthlyScores.OrderByDescending(s => s.Score);
            _redisCacheManager.Set(cacheKey, monthlyScoresOrdered);

            return monthlyScoresOrdered;
        }

        public IEnumerable<LeaderboardDto> GetAllData()
        {
            CacheKey cacheKey = new CacheKey(CacheKeys.AllDataCacheKey);

            var allDataInCache = _redisCacheManager.Get<IEnumerable<LeaderboardDto>>(cacheKey);

            if (allDataInCache != null)
            {
                return allDataInCache;
            }

            var alldata = _userScoreRepository.GetAllData();
            _redisCacheManager.Set(cacheKey, alldata);

            return _userScoreRepository.GetAllData();
        }

        public Stats GetStats()
        {
            CacheKey cacheKey = new CacheKey(CacheKeys.StatsCacheKey);
            var statsInCache = _redisCacheManager.Get<Stats>(cacheKey);

            if (statsInCache != null)
            {
                return statsInCache;
            }

            var dailyAvg = _userScoreRepository.GetDalyAvarage();
            var weeklyAvg = _userScoreRepository.GetWeeklyAvarage();
            var monthlyAvg = _userScoreRepository.GetMonthlyAvarage();
            var dailyMax = _userScoreRepository.GetDalyMax();
            var weeklyMax = _userScoreRepository.GetWeeklyMax();
            var monthlyMax = _userScoreRepository.GetMonthlyMax();

            Stats stats = new Stats
            {
                DailyAvarage = dailyAvg,
                WeeklyAvarage = weeklyAvg,
                MonthlyAvarage = monthlyAvg,
                DailyMax = dailyMax,
                WeeklyMax = weeklyMax,
                MonthlyMax = monthlyMax
            };

            _redisCacheManager.Set(cacheKey, stats);
            return stats;
        }

        public UserRating GetUserInfo(string username)
        {
            CacheKey cacheKey = new CacheKey(CacheKeys.UserInfoCacheKey, username);
            var userInfoInCache = _redisCacheManager.Get<UserRating>(cacheKey);

            if (userInfoInCache != null)
            {
                return userInfoInCache;
            }

            var currentMonthScores = _userScoreRepository
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

            var userRatingIndex = currentMonthScores.Select((Value, Index) => new { Value, Index }).SingleOrDefault(p => p.Value.Username == username);

            if (userRatingIndex == null)
            {
                throw new Exception("User Dont Have Scores");
            }

            var userRating = new UserRating
            {
                Username = userRatingIndex.Value.Username,
                Rating = userRatingIndex.Index + 1,
                Score = userRatingIndex.Value.Score
            };

            _redisCacheManager.Set(cacheKey, userRating);

            return userRating;
        }

    }
}

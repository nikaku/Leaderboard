using Leaderboard.BL.Caching;
using Leaderboard.BL.Dtos.LeaderboardDto;
using Leaderboard.BL.Dtos.UserDtos;
using Leaderboard.BL.Interfaces;
using Leaderboard.BL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Leaderboard.Services.LeaderboardServices
{
    public class LeaderboardService : ILeaderboardService
    {
        private ICacheManager _redisCacheManager;
        private IUserScoreRepository _userScoreRepository;

        public LeaderboardService(IUserScoreRepository userScoreRepository, ICacheManager staticCacheManager)
        {
            _userScoreRepository = userScoreRepository;
            _redisCacheManager = staticCacheManager;
        }

        public IEnumerable<LeaderboardDto> GetLeaderboardByDay(DateTime day, bool ignoreCache)
        {
            if (ignoreCache)
            {
                return _userScoreRepository.GetScoresByDay(day).OrderByDescending(s => s.Score);
            }
            else
            {
                CacheKey cacheKey = new CacheKey(CacheKeys.LeaderboardByDayCacheKey, day.ToString("MM-dd-yyyy"));
                var LeaderboardByDay = _redisCacheManager.Get<IEnumerable<LeaderboardDto>>(cacheKey);

                if (LeaderboardByDay != null)
                {
                    return default;
                }

                var dailyScores = _userScoreRepository.GetScoresByDay(day);

                if (dailyScores.Count() == 0)
                {
                    return dailyScores;
                }

                var dailyScoresOrdered = dailyScores.OrderByDescending(s => s.Score);

                _redisCacheManager.Set(cacheKey, dailyScoresOrdered);
                return dailyScoresOrdered;
            }
        }

        public IEnumerable<LeaderboardDto> GetLeaderboardByWeek(DateTime week, bool ignoreCache)
        {
            if (ignoreCache)
            {
                return _userScoreRepository.GetScoresByWeek(week).OrderByDescending(s => s.Score);
            }
            else
            {
                var weekNo = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(week,
                CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule,
                CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

                CacheKey cacheKey = new CacheKey(CacheKeys.LeaderboardByWeekCacheKey, $"{weekNo}-{week.Year}");
                var LeaderboardByWeek = _redisCacheManager.Get<IEnumerable<LeaderboardDto>>(cacheKey);

                if (LeaderboardByWeek != null)
                {
                    return LeaderboardByWeek;
                }

                var weeklyScores = _userScoreRepository.GetScoresByWeek(week);
                if (weeklyScores.Count() == 0)
                {
                    return default;
                }
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
        }

        public IEnumerable<LeaderboardDto> GetLeaderboardByMonth(DateTime month, bool ignoreCache)
        {
            if (ignoreCache)
            {
                return _userScoreRepository.GetScoresByMonth(month).OrderByDescending(s => s.Score);
            }
            else
            {
                CacheKey cacheKey = new CacheKey(CacheKeys.LeaderboardByMonthCacheKey, month.ToString("MM-yyyy"));
                var LeaderboardByMonth = _redisCacheManager.Get<IEnumerable<LeaderboardDto>>(cacheKey);

                if (LeaderboardByMonth != null)
                {
                    return LeaderboardByMonth;
                }

                var monthlyScores = _userScoreRepository.GetScoresByMonth(month);
                if (monthlyScores.Count() == 0)
                {
                    return default;
                }
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
        }

        public IEnumerable<LeaderboardDto> GetAllData(bool ignoreCache)
        {
            if (ignoreCache)
            {
                return _userScoreRepository.GetAllData();
            }
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

        public Stats GetStats(bool ignoreCache)
        {
            if (ignoreCache)
            {
                return new Stats
                {
                    DailyAvarage = _userScoreRepository.GetDalyAvarage(),
                    WeeklyAvarage = _userScoreRepository.GetWeeklyAvarage(),
                    MonthlyAvarage = _userScoreRepository.GetMonthlyAvarage(),
                    DailyMax = _userScoreRepository.GetDaiyMax(),
                    WeeklyMax = _userScoreRepository.GetWeeklyMax(),
                    MonthlyMax = _userScoreRepository.GetMonthlyMax()
                };
            }
            else
            {
                CacheKey cacheKey = new CacheKey(CacheKeys.StatsCacheKey);
                var statsInCache = _redisCacheManager.Get<Stats>(cacheKey);

                if (statsInCache != null)
                {
                    return statsInCache;
                }

                Stats stats = new Stats
                {
                    DailyAvarage = _userScoreRepository.GetDalyAvarage(),
                    WeeklyAvarage = _userScoreRepository.GetWeeklyAvarage(),
                    MonthlyAvarage = _userScoreRepository.GetMonthlyAvarage(),
                    DailyMax = _userScoreRepository.GetDaiyMax(),
                    WeeklyMax = _userScoreRepository.GetWeeklyMax(),
                    MonthlyMax = _userScoreRepository.GetMonthlyMax()
                };

                _redisCacheManager.Set(cacheKey, stats);
                return stats;
            }
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


            var userRatingIndex = currentMonthScores.Select((Value, Index) => new { Value, Index }).SingleOrDefault(p => p.Value.Username == username);

            if (userRatingIndex == null)
            {
                throw new Exception("User Dont Have Scores Or Doesnot Exists");
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

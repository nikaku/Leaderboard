using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.BL.Caching
{
    public static class CacheKeys
    {
        public static CacheKey StatsCacheKey => new CacheKey(Defaults.StatsPattern);
        public static CacheKey UserInfoCacheKey => new CacheKey(Defaults.UserInfoPattern);
        public static CacheKey LeaderboardByMonthCacheKey => new CacheKey(Defaults.LeaderboardByMonthPattern);
        public static CacheKey LeaderboardByWeekCacheKey => new CacheKey(Defaults.LeaderboardByWeekPattern);
        public static CacheKey LeaderboardByDayCacheKey => new CacheKey(Defaults.LeaderboardByDayPattern);
        public static CacheKey AllDataCacheKey => new CacheKey(Defaults.AllDataPattern);
    }
}

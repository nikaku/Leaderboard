using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.BL.Caching
{
    public static class Defaults
    {
        public static string StatsPattern => "Leaderboard.Stats";
        public static string UserInfoPattern => "Leaderboard.UserInfo";
        public static string LeaderboardByMonthPattern => "Leaderboard.LeaderboardByMonth";
        public static string LeaderboardByWeekPattern => "Leaderboard.LeaderboardByWeek";
        public static string LeaderboardByDaysPattern => "Leaderboard.LeaderboardByDays";
        public static string AllDataPattern => "Leaderboard.AllData";
    }
}

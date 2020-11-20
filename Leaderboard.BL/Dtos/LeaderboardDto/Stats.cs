using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.BL.Dtos.LeaderboardDto
{
    public class Stats
    {
        public int DailyAvarage { get; set; }
        public int WeeklyAvarage { get; set; }
        public int MonthlyAvarage { get; set; }
        public int DailyMax { get; set; }
        public int WeeklyMax { get; set; }
        public int MonthlyMax { get; set; }
    }
}

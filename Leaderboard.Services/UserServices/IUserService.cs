using Leaderboard.BL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.Services.UserServices
{
    public interface IUserService
    {
        public void UploadLeaderboardData(string filePath);
        public string GetLeaderboardByDay(DateTime day);
        public string GetLeaderboardByWeek(DateTime week);
        public string GetLeaderboardByMonth(DateTime month);
        public string DownloadLeaderboardByDay(DateTime day);
        public string DownloadLeaderboardByWeek(DateTime week);
        public string DownloadLeaderboardByMonth(DateTime month);
        public string GetAllData();
        public string GetStats();
        public User GetUserInfo(string username);
    }
}

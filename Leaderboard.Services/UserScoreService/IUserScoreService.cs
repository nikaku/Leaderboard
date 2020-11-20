using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.Services.UserScoreService
{
    public interface IUserScoreService
    {
        void ImportFromExcel(string path, DateTime scoreDate);
    }
}

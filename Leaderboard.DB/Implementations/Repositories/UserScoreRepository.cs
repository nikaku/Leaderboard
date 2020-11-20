using Dapper;
using Leaderboard.BL.Dtos.LeaderboardDto;
using Leaderboard.BL.Entities;
using Leaderboard.BL.Interfaces.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Leaderboard.DB.Implementations.Repositories
{
    public class UserScoreRepository : IUserScoreRepository
    {
        private IDbConnection _dbConnection;
        public UserScoreRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public IEnumerable<LeaderboardDto> GetScoresByDay(DateTime date)
        {
            string sql = $"SELECT Users.Username, UserScores.UserId, UserScores.CreateDate, UserScores.UpdateDate, UserScores.Score" +
                $" FROM UserScores " +
                $" INNER JOIN Users ON Users.Id = UserScores.UserId" +
                $" Where ScoreDate = @ScoreDate";
            IEnumerable<LeaderboardDto> records = _dbConnection.Query<LeaderboardDto>(sql, new { ScoreDate = date });
            return records;
        }

        public IEnumerable<LeaderboardDto> GetScoresByWeek(DateTime date)
        {
            string sql = $"SELECT Users.Username, UserScores.UserId, UserScores.CreateDate, UserScores.UpdateDate, UserScores.Score" +
                $" FROM UserScores " +
                $" INNER JOIN Users ON Users.Id = UserScores.UserId" +
                $" Where (DATEPART(week, ScoreDate) = DATEPART(week, @date) AND DATEPART(year, ScoreDate) = DATEPART(year, @date))";
            IEnumerable<LeaderboardDto> records = _dbConnection.Query<LeaderboardDto>(sql, new { date });
            return records;
        }

        public IEnumerable<LeaderboardDto> GetScoresByMonth(DateTime date)
        {
            string sql = $"SELECT Users.Username, UserScores.UserId, UserScores.CreateDate, UserScores.UpdateDate, UserScores.Score" +
                 $" FROM UserScores " +
                 $" INNER JOIN Users ON Users.Id = UserScores.UserId" +
                 $" Where DATEPART(month, ScoreDate) = DATEPART(month, @date)" +
                 $" AND DATEPART(year, ScoreDate) = DATEPART(year, @date)";
            IEnumerable<LeaderboardDto> records = _dbConnection.Query<LeaderboardDto>(sql, new { date });
            return records;
        }

        public IEnumerable<LeaderboardDto> GetAllData()
        {
            string sql = $"SELECT Users.Username, UserScores.UserId, UserScores.CreateDate, UserScores.UpdateDate, UserScores.Score" +
                 $" FROM UserScores " +
                 $" INNER JOIN Users ON Users.Id = UserScores.UserId";
            IEnumerable<LeaderboardDto> records = _dbConnection.Query<LeaderboardDto>(sql);
            return records;
        }

        public int GetDalyAvarage()
        {
            string sql = $"SELECT AVG(Score) FROM UserScores";
            int avgScore = _dbConnection.Query<int>(sql).FirstOrDefault();
            return avgScore;
        }

        public int GetWeeklyAvarage()
        {
            string sql = $"SELECT AVG(Score) [WeeklyAvarage] " +
                $"FROM (SELECT DATEPART(week, ScoreDate) [Week], SUM(Score)[Score]" +
                $"    FROM UserScores" +
                $"    GROUP BY DATEPART(week, ScoreDate)) weeklySum";
            int avgScore = _dbConnection.Query<int>(sql).FirstOrDefault();
            return avgScore;
        }

        public int GetMonthlyAvarage()
        {
            string sql = $"SELECT AVG(Score) [WeeklyAvarage] " +
               $"FROM (SELECT DATEPART(month, ScoreDate) [Month], SUM(Score)[Score]" +
               $"    FROM UserScores" +
               $"    GROUP BY DATEPART(month, ScoreDate)) weeklySum";
            int avgScore = _dbConnection.Query<int>(sql).FirstOrDefault();
            return avgScore;
        }

        public int GetDalyMax()
        {
            string sql = $"SELECT MAX(Score) FROM UserScores";
            int maxScore = _dbConnection.Query<int>(sql).FirstOrDefault();
            return maxScore;
        }

        public int GetWeeklyMax()
        {
            string sql = $"SELECT MAX(Score) [WeeklyAvarage] " +
                $"FROM (SELECT DATEPART(week, ScoreDate) [Week], SUM(Score)[Score]" +
                $"    FROM UserScores" +
                $"    GROUP BY DATEPART(week, ScoreDate)) weeklySum";
            int maxScore = _dbConnection.Query<int>(sql).FirstOrDefault();
            return maxScore;
        }

        public int GetMonthlyMax()
        {
            string sql = $"SELECT MAX(Score) [WeeklyAvarage] " +
               $"FROM (SELECT DATEPART(month, ScoreDate) [Month], SUM(Score)[Score]" +
               $"    FROM UserScores" +
               $"    GROUP BY DATEPART(month, ScoreDate)) weeklySum";
            int maxScore = _dbConnection.Query<int>(sql).FirstOrDefault();
            return maxScore;
        }
    }
}

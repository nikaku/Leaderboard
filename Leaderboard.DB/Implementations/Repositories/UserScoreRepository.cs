using Dapper;
using Leaderboard.BL.Dtos.LeaderboardDto;
using Leaderboard.BL.Interfaces;
using Leaderboard.BL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Leaderboard.DB.Implementations.Repositories
{
    public class UserScoreRepository : IUserScoreRepository
    {
        private IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;
        public UserScoreRepository(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<LeaderboardDto> GetScoresByDay(DateTime date)
        {
            string sql = $"SELECT Users.Username, UserScores.UserId, UserScores.CreateDate, UserScores.UpdateDate, UserScores.Score" +
                $" FROM UserScores " +
                $" INNER JOIN Users ON Users.Id = UserScores.UserId" +
                $" Where ScoreDate = @ScoreDate";
            IEnumerable<LeaderboardDto> records = _unitOfWork.Connection.Query<LeaderboardDto>(sql, new { ScoreDate = date });
            return records;
        }

        public IEnumerable<LeaderboardDto> GetScoresByWeek(DateTime date)
        {
            string sql = $"SELECT Users.Username, UserScores.UserId, UserScores.CreateDate, UserScores.UpdateDate, UserScores.Score" +
                $" FROM UserScores " +
                $" INNER JOIN Users ON Users.Id = UserScores.UserId" +
                $" Where (DATEPART(week, ScoreDate) = DATEPART(week, @date) AND DATEPART(year, ScoreDate) = DATEPART(year, @date))";
            IEnumerable<LeaderboardDto> records = _unitOfWork.Connection.Query<LeaderboardDto>(sql, new { date });
            return records;
        }

        public IEnumerable<LeaderboardDto> GetScoresByMonth(DateTime date)
        {
            string sql = $"SELECT Users.Username, UserScores.UserId, UserScores.CreateDate, UserScores.UpdateDate, UserScores.Score" +
                 $" FROM UserScores " +
                 $" INNER JOIN Users ON Users.Id = UserScores.UserId" +
                 $" Where DATEPART(month, ScoreDate) = DATEPART(month, @date)" +
                 $" AND DATEPART(year, ScoreDate) = DATEPART(year, @date)";
            IEnumerable<LeaderboardDto> records = _unitOfWork.Connection.Query<LeaderboardDto>(sql, new { date });
            return records;
        }

        public IEnumerable<LeaderboardDto> GetAllData()
        {
            string sql = $"SELECT Users.Username, UserScores.UserId, UserScores.CreateDate, UserScores.UpdateDate, UserScores.Score" +
                 $" FROM UserScores " +
                 $" INNER JOIN Users ON Users.Id = UserScores.UserId";
            IEnumerable<LeaderboardDto> records = _unitOfWork.Connection.Query<LeaderboardDto>(sql);
            return records;
        }

        public int GetDalyAvarage()
        {
            string sql = $"SELECT SUM(Score)/COUNT(distinct(ScoreDate)) FROM UserScores";
            int avgScore = _unitOfWork.Connection.Query<int>(sql).FirstOrDefault();
            return avgScore;
        }

        public int GetWeeklyAvarage()
        {
            string sql = $"SELECT AVG(Score) [WeeklyAvarage] " +
                $"FROM (SELECT DATEPART(week, ScoreDate) [Week], SUM(Score)[Score]" +
                $"    FROM UserScores" +
                $"    GROUP BY DATEPART(week, ScoreDate), DATEPART(year,ScoreDate)) weeklySum";
            int avgScore = _unitOfWork.Connection.Query<int>(sql).FirstOrDefault();
            return avgScore;
        }

        public int GetMonthlyAvarage()
        {
            string sql = $"SELECT AVG(Score) [MonthlyAvarage] " +
               $"FROM (SELECT DATEPART(month, ScoreDate) [Month], SUM(Score)[Score]" +
               $"    FROM UserScores" +
               $"    GROUP BY DATEPART(month, ScoreDate), DATEPART(year,ScoreDate)) monthlyAvarage";
            int avgScore = _unitOfWork.Connection.Query<int>(sql).FirstOrDefault();
            return avgScore;
        }

        public int GetDaiyMax()
        {
            string sql = $"SELECT MAX(Score) FROM UserScores";
            int maxScore = _unitOfWork.Connection.Query<int>(sql).FirstOrDefault();
            return maxScore;
        }

        public int GetWeeklyMax()
        {
            string sql = $"SELECT MAX(Score) [WeeklyMax] " +
                $"FROM (SELECT DATEPART(week, ScoreDate) [Week], SUM(Score)[Score]" +
                $"    FROM UserScores" +
                $"    GROUP BY DATEPART(week, ScoreDate), DATEPART(year,ScoreDate)) weeklySum";
            int maxScore = _unitOfWork.Connection.Query<int>(sql).FirstOrDefault();
            return maxScore;
        }

        public int GetMonthlyMax()
        {
            string sql = $"SELECT MAX(Score) [MonthlyMax] " +
               $"FROM (SELECT DATEPART(month, ScoreDate) [Month], SUM(Score)[Score]" +
               $"    FROM UserScores" +
               $"    GROUP BY DATEPART(month, ScoreDate)) monthlySum";
            int maxScore = _unitOfWork.Connection.Query<int>(sql).FirstOrDefault();
            return maxScore;
        }

        public void AddOrUpdate(LeaderboardDto leaderboardDto, DateTime scoreDate)
        {
            string sql = string.Empty;
            var user = _userRepository.GetByUsername(leaderboardDto.Username);

            if (user == null)
            {
                sql = @" DECLARE @id INT;
                         BEGIN TRY
                         BEGIN TRANSACTION
                         INSERT INTO Users(Username)  VALUES(@Username) set @id = (SELECT SCOPE_IDENTITY())
                         INSERT INTO UserScores(UserId, Score, ScoreDate, CreateDate) VALUES(@id, @Score, @ScoreDate, @CreateDate)
                         COMMIT TRANSACTION
                         END TRY
                         BEGIN CATCH
                         ROLLBACK TRANSACTION
                         END CATCH";

                _unitOfWork.Connection.Query<int>(sql,
                    new
                    {
                        Username = leaderboardDto.Username,
                        Score = leaderboardDto.Score,
                        ScoreDate = scoreDate,
                        CreateDate = DateTime.Now
                    }
                ).FirstOrDefault();
            }
            else
            {
                bool updateFlag = GetByDate(scoreDate, user.Id);
                if (updateFlag)
                {
                    sql = "UPDATE UserScores Set Score = @Score, UpdateDate = @UpdateDate WHERE UserId = @UserId";
                    _unitOfWork.Connection.Query<int>(sql,
                        new
                        {
                            UserId = user.Id,
                            Score = leaderboardDto.Score,
                            Updatedate = DateTime.Now
                        }
                    ).FirstOrDefault();
                }
                else
                {
                    sql = "INSERT INTO UserScores(UserId, Score, ScoreDate, CreateDate) VALUES(@id, @Score, @ScoreDate, @CreateDate)";
                    _unitOfWork.Connection.Query<int>(sql,
                        new
                        {
                            id = user.Id,
                            Score = leaderboardDto.Score,
                            ScoreDate = scoreDate,
                            CreateDate = DateTime.Now
                        }
                    ).FirstOrDefault();
                }
            }
        }

        public bool GetByDate(DateTime date, int userId)
        {
            string sql = $"SELECT CASE WHEN EXISTS (" +
                $"SELECT * FROM UserScores WHERE ScoreDate = '{date:s}' AND UserID = {userId})" +
                $"THEN CAST(1 AS BIT)" +
                $"ELSE CAST(0 AS BIT) END";
            return _unitOfWork.Connection.Query<bool>(sql).First();
        }
    }
}

using ExcelHelper;
using ExcelHelper.ExportImport;
using Leaderboard.BL.Dtos.LeaderboardDto;
using Leaderboard.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Leaderboard.Services.UserScoreService
{
    public class UserScoreService : IUserScoreService
    {
        private IUnitOfWork _unitOfWork;

        public UserScoreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public void ImportFromExcel(string path, DateTime scoreDate)
        //{
        //    var excelImport = new ImportManager();
        //    var sheetNames = excelImport.ToExcelsSheetList(path);
        //    var userScoreDt = excelImport.ReadExcelFile(sheetNames.First(), path);

        //    var userScores = new List<LeaderboardDto>();

        //    foreach (DataRow row in userScoreDt.Rows)
        //    {
        //        userScores.Add(new LeaderboardDto
        //        {
        //            Username = row["F1"].ToString(),
        //            Score = int.Parse(row["F2"].ToString())
        //        });
        //    }

        //    foreach (var userScore in userScores)
        //    {
        //        _unitOfWork.UserScoreRepository.Add(userScore, scoreDate);
        //    }
        //}
    }
}

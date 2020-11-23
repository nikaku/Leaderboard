using ExcelHelper.ExportImport;
using Leaderboard.BL.Caching;
using Leaderboard.BL.Dtos.LeaderboardDto;
using Leaderboard.BL.Interfaces;
using Leaderboard.BL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Leaderboard.Services.ImportExport
{
    public class ImportExportService : IImportExportService
    {
        private IImportManager _importManager;
        private IExportManager _exportManager;
        private IStaticCacheManager _redisCacheManager;
        private IUserScoreRepository _userScoreRepository;

        public ImportExportService(
            IImportManager importManager,
            IExportManager exportManager,
            IStaticCacheManager staticCacheManager,
            IUserScoreRepository userScoreRepository)
        {
            _importManager = importManager;
            _exportManager = exportManager;
            _redisCacheManager = staticCacheManager;
            _userScoreRepository = userScoreRepository;
        }

        public void ExportToExcel<T>(IEnumerable<T> items, string path)
        {
            _exportManager.ExportToExcel(items, path);
        }

        public void ExportToExcel<T>(T model, string path)
        {
            _exportManager.ExportToExcel(model, path);
        }

        public void ImportFromExcel(string path, DateTime scoreDate)
        {
            var sheetNames = _importManager.ToExcelsSheetList(path);
            var userScoreDt = _importManager.ReadExcelFile(sheetNames.First(), path);
            var userScores = new List<LeaderboardDto>();

            foreach (DataRow row in userScoreDt.Rows)
            {
                userScores.Add(new LeaderboardDto
                {
                    Username = row["F1"].ToString(),
                    Score = int.Parse(row["F2"].ToString())
                });
            }

            foreach (var userScore in userScores)
            {
                _userScoreRepository.AddOrUpdate(userScore, scoreDate);
            }

            _redisCacheManager.ClearCache(Defaults.UserInfoPattern, userScores.Select(u => u.Username));
        }
    }

}

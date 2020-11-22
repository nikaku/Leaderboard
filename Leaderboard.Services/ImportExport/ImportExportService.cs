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

        private IUnitOfWork _unitOfWork;
        private IImportManager _importManager;
        private IExportManager _exportManager;
        private IStaticCacheManager _redisCacheManager;
        private IUserScoreRepository _userScoreRepository;

        public ImportExportService(IUnitOfWork unitOfWork,
            IImportManager importManager,
            IExportManager exportManager,
            IStaticCacheManager staticCacheManager,
            IUserScoreRepository userScoreRepository)
        {
            _unitOfWork = unitOfWork;
            _importManager = importManager;
            _exportManager = exportManager;
            _redisCacheManager = staticCacheManager;
            _userScoreRepository = userScoreRepository;
        }


        public byte[] ExportToExcel(IEnumerable<LeaderboardDto> leaderboardDtos)
        {
            _exportManager.ExportToExcel(leaderboardDtos);
            return new byte[] { };
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

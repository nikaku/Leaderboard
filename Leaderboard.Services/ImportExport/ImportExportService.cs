using ExcelHelper.ExportImport;
using Leaderboard.BL.Dtos.LeaderboardDto;
using Leaderboard.BL.Interfaces;
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

        public ImportExportService(IUnitOfWork unitOfWork, IImportManager _importManager, IExportManager exportManager)
        {
            _unitOfWork = unitOfWork;
            this._importManager = _importManager;
            _exportManager = exportManager;
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
                _unitOfWork.UserScoreRepository.Add(userScore, scoreDate);
            }
        }
    }

}

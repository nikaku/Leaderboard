using Leaderboard.BL.Dtos.LeaderboardDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.Services.ImportExport
{
    public interface IImportExportService
    {
        void ImportFromExcel(string path, DateTime scoreDate);
        void ExportToExcel<T>(IEnumerable<T> collection, string path);
        void ExportToExcel<T>(T item, string path);
    }
}

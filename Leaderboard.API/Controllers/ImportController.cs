using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Leaderboard.Services.ImportExport;
using Leaderboard.Services.UserScoreService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private IImportExportService _importExportService;
        public ImportController(IImportExportService importExportService)
        {
            _importExportService = importExportService;
        }

        [HttpPost]
        public IActionResult UploadLeaderboardData(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("Please Select File");
            }

            var fileExtension = file.FileName.Split(".")[1];
            var fileDate = file.FileName.Split(".")[0];
            DateTime scoreDate = DateTime.MinValue;
            if (!fileExtension.EndsWith(".xlsx") &&
                !fileExtension.EndsWith(".xls") &&
                !DateTime.TryParse(fileDate, out scoreDate))
            {
                return BadRequest("File Format Should Be \".xlsx\" Or \".xls\" And Date Fromat MM-dd-yyyy");
            }

            string path = Path.Combine(Path.GetTempPath(), "ImportedExcelFiles");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Path.GetFileName(file.FileName);

            string filePath = Path.Combine(path, fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            _importExportService.ImportFromExcel(filePath, scoreDate);

            return Ok();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Leaderboard.Services.UserScoreService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private IUserScoreService _userScoreService;
        public ImportController(IUserScoreService userScoreService)
        {
            _userScoreService = userScoreService;
        }

        [HttpPost]
        public IActionResult UploadLeaderboardData(IFormFile file)
        {
            var fileExtension = file.FileName.Split(".")[1];
            var fileDate = file.FileName.Split(".")[0];
            DateTime scoreDate = DateTime.MinValue;
            if (!fileExtension.EndsWith(".xlsx") &&
                !fileExtension.EndsWith(".xls") &&
                !DateTime.TryParse(fileDate, out scoreDate))
            {
                return BadRequest();
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


            _userScoreService.ImportFromExcel(filePath, scoreDate);

            return Ok();
        }
    }
}

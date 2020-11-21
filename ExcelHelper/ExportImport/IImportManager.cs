using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ExcelHelper.ExportImport
{
    public interface IImportManager
    {
        DataTable ReadExcelFile(string sheetName, string path);
        List<string> ToExcelsSheetList(string excelFilePath);
    }
}

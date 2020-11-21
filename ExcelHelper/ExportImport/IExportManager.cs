using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ExcelHelper.ExportImport
{
    public interface IExportManager
    {
        void ExportToExcel<T>(IEnumerable<T> items);
    }
}

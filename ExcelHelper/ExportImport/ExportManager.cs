using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Text;

namespace ExcelHelper.ExportImport
{
    public class ExportManager : IExportManager
    {
        private DataTable IEnumerableToDatatable<T>(IEnumerable<T> items)
        {
            var table = CreateDataTableForPropertiesOfType<T>();
            PropertyInfo[] piT = typeof(T).GetProperties();

            foreach (var item in items)
            {
                var dr = table.NewRow();

                for (int property = 0; property < table.Columns.Count; property++)
                {
                    if (piT[property].CanRead)
                    {
                        dr[property] = piT[property].GetValue(item, null);
                    }
                }

                table.Rows.Add(dr);
            }
            return table;
        }
        public static DataTable CreateDataTableForPropertiesOfType<T>()
        {
            DataTable dt = new DataTable();
            PropertyInfo[] piT = typeof(T).GetProperties();

            foreach (PropertyInfo pi in piT)
            {
                Type propertyType = null;
                if (pi.PropertyType.IsGenericType)
                {
                    propertyType = pi.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    propertyType = pi.PropertyType;
                }
                DataColumn dc = new DataColumn(pi.Name, propertyType);

                if (pi.CanRead)
                {
                    dt.Columns.Add(dc);
                }
            }

            return dt;
        }

        public void ExportToExcel<T>(IEnumerable<T> items)
        {
            var dataTable = IEnumerableToDatatable(items);
            string Import_FileName = @"C:\Users\nikak\Downloads";

            using (OleDbConnection conn = new OleDbConnection())
            {
                conn.ConnectionString = "Provider =Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=NO;IMEX=1'";

                string sheetName = "Seet1";

                using (OleDbCommand comm = new OleDbCommand())
                {
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = "insert into Items ([Item_Name],[Item_Price]) values (1,2)";
                    comm.Connection = conn;
                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;

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

        private DataTable ObjectToDatatable<T>(T item)
        {
            var table = CreateDataTableForPropertiesOfType<T>();
            PropertyInfo[] piT = typeof(T).GetProperties();
            var dr = table.NewRow();
            for (int property = 0; property < table.Columns.Count; property++)
            {
                if (piT[property].CanRead)
                {
                    dr[property] = piT[property].GetValue(item, null);
                }
            }
            table.Rows.Add(dr);

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

        public void ExportToExcel<T>(IEnumerable<T> items, string path)
        {
            var dataTable = IEnumerableToDatatable(items);
            string provider = "Microsoft.ACE.OLEDB.12.0";
            string connectionString = $"Provider={provider};Data Source={path};Extended Properties='Excel 12.0 Xml;HDR=Yes';";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                using (OleDbCommand comm = new OleDbCommand())
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn dataColumn in dataTable.Columns)
                    {
                        columnNames.Add(dataColumn.ColumnName);
                    }

                    string tableName = !string.IsNullOrWhiteSpace(dataTable.TableName) ? dataTable.TableName : Guid.NewGuid().ToString();
                    comm.CommandText = $"CREATE TABLE [{tableName}] ({string.Join(",", columnNames.Select(c => $"[{c}] VARCHAR").ToArray())});"; comm.Connection = conn;
                    comm.ExecuteNonQuery();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        List<string> rowValues = new List<string>();
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            rowValues.Add((row[column] != null && row[column] != DBNull.Value) ? row[column].ToString() : String.Empty);
                        }
                        comm.CommandText = $"INSERT INTO [{tableName}]({String.Join(",", columnNames.Select(c => $"[{c}]"))}) VALUES ({String.Join(",", rowValues.Select(r => $"'{r}'").ToArray())});";
                        comm.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
        }

        public void ExportToExcel<T>(T item, string path)
        {
            var dataTable = ObjectToDatatable(item);
            string provider = "Microsoft.ACE.OLEDB.12.0";
            string connectionString = $"Provider={provider};Data Source={path};Extended Properties='Excel 12.0 Xml;HDR=Yes';";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                using (OleDbCommand comm = new OleDbCommand())
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn dataColumn in dataTable.Columns)
                    {
                        columnNames.Add(dataColumn.ColumnName);
                    }

                    string tableName = !string.IsNullOrWhiteSpace(dataTable.TableName) ? dataTable.TableName : Guid.NewGuid().ToString();
                    comm.CommandText = $"CREATE TABLE [{tableName}] ({string.Join(",", columnNames.Select(c => $"[{c}] VARCHAR").ToArray())});"; comm.Connection = conn;
                    comm.ExecuteNonQuery();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        List<string> rowValues = new List<string>();
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            rowValues.Add((row[column] != null && row[column] != DBNull.Value) ? row[column].ToString() : String.Empty);
                        }
                        comm.CommandText = $"INSERT INTO [{tableName}]({String.Join(",", columnNames.Select(c => $"[{c}]"))}) VALUES ({String.Join(",", rowValues.Select(r => $"'{r}'").ToArray())});";
                        comm.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
        }
    }
}

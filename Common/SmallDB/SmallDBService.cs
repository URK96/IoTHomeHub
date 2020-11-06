using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

namespace SmallDB
{
    public static class SmallDBService
    {
        public static DataTable DBTable { get; private set; }
        public static string DeviceDBFile => Path.Combine(dbPath.FullName, "DeviceDB");

        private static DirectoryInfo dbPath = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "DB"));

        public static void CreateDB()
        {
            try
            {
                DBTable = new DataTable("IoTDeviceDB");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public static bool LoadDB()
        {
            try
            {
                DBTable.Clear();
                DBTable.ReadXml(DeviceDBFile);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }

        public static bool SaveDB()
        {
            try
            {
                if (!dbPath.Exists)
                {
                    dbPath.Create();
                }

                DBTable.AcceptChanges();
                DBTable.WriteXml(DeviceDBFile, XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }

        public static List<DataRow> FindDataRow<T>(string index, T value)
        {
            try
            {
                var rows = from row in DBTable.AsEnumerable() where CheckEqual(row, index, value) select row;

                return new List<DataRow>(rows);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                return null;
            }
        }

        private static bool CheckEqual<T>(DataRow dr, string index, T value)
        {
            try
            {
                if(CheckDBNull(dr, index))
                {
                    throw new Exception("This index content is DBNull.");
                }

                return ((T)dr[index]).Equals(value);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                return false;
            }
        }

        private static bool CheckDBNull(DataRow dr, string index) => dr[index] == DBNull.Value;
    }
}

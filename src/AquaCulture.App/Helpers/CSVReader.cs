using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AquaCulture.App.Helpers
{
    public class CSVReader
    {
        public static DataTable ConvertCSVtoDataTable(string strFilePath, bool IsLinux=false)
        {
            if (!System.IO.File.Exists(strFilePath)) return null;
            var _temp = string.Empty;
            if (IsLinux)
            {
                _temp = strFilePath.Replace("\\", "/");
            }
            else
            {
                _temp = strFilePath;
            }
            

            StreamReader sr = new StreamReader(_temp);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dt = new DataTable("data")
            {

            };
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}

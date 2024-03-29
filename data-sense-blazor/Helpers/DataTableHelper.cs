﻿using System.Data;
using System.Text.RegularExpressions;

namespace data_sense_blazor.Helpers
{
    public class DataTableHelper
    {
        public static List<Dictionary<string, object>> ConvertDataTable(DataTable dt)
        {
            var convertedList = new List<Dictionary<string, object>>();

            foreach (DataRow row in dt.Rows)
            {
                var rowDictionary = new Dictionary<string, object>();

                foreach (DataColumn column in dt.Columns)
                {
                    if (row[column] == DBNull.Value)
                    {
                        rowDictionary[column.ColumnName] = null;
                    }
                    else
                    {
                        rowDictionary[column.ColumnName] = row[column];
                    }
                }

                convertedList.Add(rowDictionary);
            }

            return convertedList;
        }
        public static string FormatSQL(string query)
        {
            query = Regex.Replace(query, @"\s+", " ");

            string[] keywords = new[] { "SELECT", "FROM", "WHERE", "GROUP BY", "HAVING", "ORDER BY", "INNER JOIN", "LEFT JOIN", "RIGHT JOIN" };
            foreach (var keyword in keywords)
            {
                query = query.Replace(keyword, "\n" + keyword);
            }

            return query;
        }

    }
}

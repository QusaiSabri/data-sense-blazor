using System.Data;

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
                    rowDictionary[column.ColumnName] = row[column];
                }

                convertedList.Add(rowDictionary);
            }

            return convertedList;
        }
    }
}

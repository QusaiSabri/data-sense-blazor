using System.Data;
using data_sense_blazor.Models;

namespace data_sense_blazor.Interfaces
{
    public interface IDatabaseService
    {
        Task<List<Database>> GetDatabases();
        Task<List<Table>> GetTables(string databaseName);
        Task<List<Column>> GetColumns(string databaseName, string tableName);
        Task<DataTable> ExecuteQuery(string query);
        Task<DataTable> ExecuteGroupBy(List<string> column, Table table, string database);
        Task<string> GenerateQuery(List<string> column, Table table, string database);
    }
}

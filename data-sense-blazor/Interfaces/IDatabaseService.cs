using data_sense_blazor.Models;

namespace data_sense_blazor.Interfaces
{
    public interface IDatabaseService
    {
        public List<Database> GetDatabases();
        public List<Table> GetTables(string connectionString, string databaseName);
        public List<Column> GetColumns(string connectionString, string databaseName, string tableName);

    }
}

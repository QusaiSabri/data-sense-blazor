using data_sense_blazor.Models;

namespace data_sense_blazor.Interfaces
{
    public interface IDatabaseService
    {
        public Task<List<Database>> GetDatabases();
        public Task<List<Table>> GetTables(string databaseName);
        public Task<List<Column>> GetColumns(string databaseName, string tableName);

    }
}

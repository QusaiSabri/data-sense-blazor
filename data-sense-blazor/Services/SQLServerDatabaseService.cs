using data_sense_blazor.Interfaces;
using data_sense_blazor.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

public class SQLServerDatabaseService : IDatabaseService
{
    private readonly string _connectionString;
    private readonly ILogger<SQLServerDatabaseService> _logger;

    public SQLServerDatabaseService(string connectionString, ILogger<SQLServerDatabaseService> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
        _logger.LogInformation("SQLServerDatabaseService started.");
    }

    public async Task<List<Database>> GetDatabases()
    {
        _logger.LogInformation("Getting databases.");

        var databases = new List<Database>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("SELECT name FROM sys.databases;", connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var database = new Database
                        {
                            Name = reader.GetString(0),
                        };

                        databases.Add(database);
                        _logger.LogDebug("Added database: {database}", database.Name);
                    }
                }
            }
        }

        _logger.LogInformation("Finished getting databases.");
        return databases;
    }

    public async Task<List<Table>> GetTables(string databaseName)
    {
        _logger.LogInformation("Getting tables for database: {databaseName}", databaseName);

        var tables = new List<Table>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand($"USE {databaseName}; SELECT name FROM sys.tables;", connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var table = new Table
                        {
                            Name = reader.GetString(0)
                        };

                        tables.Add(table);
                        _logger.LogDebug("Added table: {table} to database: {databaseName}", table.Name, databaseName);
                    }
                }
            }
        }

        _logger.LogInformation("Finished getting tables for database: {databaseName}.", databaseName);
        return tables;
    }

    public async Task<List<Column>> GetColumns(string databaseName, string tableName)
    {
        _logger.LogInformation("Getting columns for table: {tableName} in database: {databaseName}", tableName, databaseName);

        var columns = new List<Column>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand($"USE {databaseName}; SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName;", connection))
            {
                command.Parameters.AddRange(new[]
                {
                    new SqlParameter("@TableName", SqlDbType.NVarChar) { Value = tableName }
                });

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var column = new Column
                        {
                            Name = reader.GetString(0),
                            DataType = reader.GetString(1),
                            IsNullable = reader.GetString(2) == "YES"
                        };

                        columns.Add(column);
                        _logger.LogDebug("Added column: {column} to table: {tableName} in database: {databaseName}", column.Name, tableName, databaseName);
                    }
                }
            }
        }

        _logger.LogInformation("Finished getting columns for table: {tableName} in database: {databaseName}", tableName, databaseName);
        return columns;
    }
}

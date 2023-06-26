using data_sense_blazor.Interfaces;
using data_sense_blazor.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

public class SQLServerDatabaseService : IDatabaseService
{
    private readonly string _connectionString;
   
    public SQLServerDatabaseService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Database>> GetDatabases()
    {
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
                            //Tables = await GetTables(reader.GetString(0))
                        };

                        databases.Add(database);
                    }
                }
            }
        }

        return databases;
    }

    public async Task<List<Table>> GetTables(string databaseName)
    {
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
                    }
                }
            }
        }

        return tables;
    }

    public async Task<List<Column>> GetColumns(string databaseName, string tableName)
    {
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
                    }
                }
            }
        }

        return columns;
    }
}

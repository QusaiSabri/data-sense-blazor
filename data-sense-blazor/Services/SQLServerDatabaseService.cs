using System.Data;
using data_sense_blazor.Interfaces;
using data_sense_blazor.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

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

            using (var command = new SqlCommand($"USE {databaseName}; SELECT TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';", connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var table = new Table
                        {
                            SchemaName = reader.GetString(0),
                            Name = reader.GetString(1)
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

    public async Task<DataTable> ExecuteQuery(string query)
    {
        var conn = new SqlConnection(_connectionString);
        DataTable dataTable = new DataTable();

        using (SqlCommand command = new SqlCommand(query, conn))
        {
            try
            {
                await conn.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        return dataTable;
    }

    public async Task<DataTable> ExecuteGroupBy(List<string> columns, Table table, string database)
    {
        var conn = new SqlConnection(_connectionString);

        if (conn.State != ConnectionState.Open)
        {
            await conn.OpenAsync();
        }

        DataTable dataTable = new DataTable();
        try
        {
            //var columnNames = string.Join(", ", columns.Select(c => $"[{c}]"));

            //string commandText = $"SELECT COUNT(*) AS Count, {columnNames} FROM [{database}].[{table.SchemaName}].[{table.Name}] GROUP BY {columnNames}";

            string commandText = $"SELECT COUNT(*) AS Count, " +
                     string.Join(", ", columns.Select(c => $"ISNULL([{c}], 'NULL') AS [{c}]")) +
                     $" FROM [{database}].[{table.SchemaName}].[{table.Name}] GROUP BY " +
                     string.Join(", ", columns.Select(c => $"[{c}]"));


            using (SqlCommand command = new SqlCommand(commandText, conn))
            {
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.FillSchema(dataTable, SchemaType.Source);
                da.Fill(dataTable);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        return dataTable;
    }
}

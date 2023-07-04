using System.Data;
using data_sense_blazor.Interfaces;
using data_sense_blazor.Models;
using Microsoft.Data.SqlClient;

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

    public async Task<DataTable> ExecuteGroupBy(string column, string table, string database)
    {
        var conn = new SqlConnection(_connectionString);

        if (conn.State != ConnectionState.Open)
        {
            await conn.OpenAsync();
        }

        DataTable dataTable = new DataTable();
        try
        {
            using (SqlCommand command = new SqlCommand("SELECT COUNT(*), @Column FROM @Database.@Table GROUP BY @Column", conn))
            {
                command.Parameters.AddWithValue("@Column", column);
                command.Parameters.AddWithValue("@Table", table);
                command.Parameters.AddWithValue("@Database", database);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
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

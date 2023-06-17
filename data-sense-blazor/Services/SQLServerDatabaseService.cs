using data_sense_blazor.Interfaces;
using data_sense_blazor.Models;
using Microsoft.Data.SqlClient;

namespace data_sense_blazor.Services
{
    public class SQLServerDatabaseService : IDatabaseService
    {
        private readonly string _connectionString;
        public SQLServerDatabaseService(string connectionString = "Server=.;Database=.;Trusted_Connection=true;TrustServerCertificate=true;")
        {
            _connectionString = connectionString;
        }
        public List<Database> GetDatabases()
        {
            var databases = new List<Database>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT name FROM sys.databases where name = '.';", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var database = new Database
                            {
                                Name = reader.GetString(0),
                                Tables = GetTables(_connectionString, reader.GetString(0))
                            };

                            databases.Add(database);
                        }
                    }
                }
            }

            return databases;
        }

        public List<Table> GetTables(string connectionString, string databaseName)
        {
            var tables = new List<Table>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand($"USE {databaseName}; SELECT name FROM sys.tables;", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var table = new Table
                            {
                                Name = reader.GetString(0),
                                Columns = GetColumns(connectionString, databaseName, reader.GetString(0)) // Get columns for each table
                            };

                            tables.Add(table);
                        }
                    }
                }
            }

            return tables;
        }


        public List<Column> GetColumns(string connectionString, string databaseName, string tableName)
        {
            var columns = new List<Column>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand($"USE {databaseName}; SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}';", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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
}

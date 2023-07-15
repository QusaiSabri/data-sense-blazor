using data_sense_blazor.Interfaces;
using data_sense_blazor.Models;
using data_sense_blazor.Shared;
using Microsoft.Data.SqlClient;

namespace data_sense_blazor.Services
{
    public class SqlServerConnectionService : IDatabaseConnectionService
    {
        private readonly AppState _appState;
        public SqlServerConnectionService(AppState appState)
        {
            _appState = appState;
        }
        public async Task<(bool isConnected, string message)> Connect(DatabaseConfiguration config)
        {
            try
            {
                string connectionString = $"Server={config.ServerName};Database={config.DatabaseName};Trusted_Connection=true;Trusted_Connection=true;TrustServerCertificate=true";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    _appState.SetConnectionString(connectionString);
                    _appState.SetConnectionStatus(true);

                    return (isConnected: true, message: "");
                }
            }
            catch (SqlException ex)
            {
                _appState.SetConnectionStatus(false);
                return (isConnected: false, message: ex.Message);
            }
        }
    }
}

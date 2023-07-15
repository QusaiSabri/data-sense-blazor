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
                string connectionString = BuildConnectionString(config);

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

        private string BuildConnectionString(DatabaseConfiguration config)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = config.ServerName,
                InitialCatalog = string.IsNullOrWhiteSpace(config.DatabaseName) ? "master" : config.DatabaseName,
                IntegratedSecurity = string.IsNullOrWhiteSpace(config.Password) ? true : false,
                TrustServerCertificate = config.TrustServerCertificate,
                UserID = config.UserId ?? "",
                Password = config.Password ?? "",
            };

            return builder.ConnectionString;
        }

    }
}

using data_sense_blazor.Interfaces;
using data_sense_blazor.Services;

namespace data_sense_blazor.Factories
{
    public static class ConnectionServiceFactory
    {
        public static IDatabaseConnectionService GetConnectionService(string selectedDatabaseType, IServiceProvider services)
        {
            switch (selectedDatabaseType)
            {
                case "SQLServer":
                    return services.GetRequiredService<SqlServerConnectionService>();
                // Add more cases for other types of databases
                default:
                    throw new ArgumentException($"Unsupported database type: {selectedDatabaseType}");
            }
        }
    }
}

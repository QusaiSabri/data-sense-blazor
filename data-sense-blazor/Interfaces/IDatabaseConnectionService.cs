using data_sense_blazor.Models;

namespace data_sense_blazor.Interfaces
{
    public interface IDatabaseConnectionService
    {
        Task<(bool isConnected, string message)> Connect(DatabaseConfiguration config);
    }
}

using System.Data;
using data_sense_blazor.Models;

namespace data_sense_blazor.Shared
{
    public class AppState
    {
        public string ConnectionString { get; private set; } = String.Empty;
        public bool IsConnected { get; private set; } = false;
        public string Query { get; private set; } = String.Empty;
        public DataTable QueryResult { get; private set; } = new DataTable();
        public List<string> ColumnsSelected { get; private set; } = new List<string>();
        public Table SelectedTable { get; private set; } = new Table();
        public string SelectedDatabase { get; private set; } = String.Empty;

        public event Action OnConnectionStringChange;
        public event Action OnConnectionStatusChange;
        public event Action OnQueryResultChange;
        public event Action OnQueryChange;
        public event Action OnColumnsSelectedChanged;
        public event Action OnSelectedTableChange;
        public event Action OnSelectedDatabaseChange;

        public void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
            OnConnectionStringChange?.Invoke();
        }
        public void SetConnectionStatus(bool status)
        {
            IsConnected = status;
            OnConnectionStatusChange?.Invoke();
        }
        public void AddToColumnsSelected(string column)
        {
            ColumnsSelected.Add(column);
            OnColumnsSelectedChanged?.Invoke();
        }

        public void RemoveFromColumnsSelected(string column)
        {
            ColumnsSelected.Remove(column);
            OnColumnsSelectedChanged?.Invoke();
        }


        public void SetQueryResult(DataTable queryResult)
        {
            QueryResult = queryResult;
            OnQueryResultChange?.Invoke();
        }

        public void SetQuery(string query)
        {
            Query = query;
            OnQueryChange?.Invoke();
        }

        public void SetColumnsSelected(List<string> columnsSelected)
        {
            ColumnsSelected = columnsSelected;
            OnColumnsSelectedChanged?.Invoke();
        }

        public void SetSelectedTable(Table selectedTable)
        {
            SelectedTable = selectedTable;
            OnSelectedTableChange?.Invoke();
        }

        public void SetSelectedDatabase(string selectedDatabase)
        {
            SelectedDatabase = selectedDatabase;
            OnSelectedDatabaseChange?.Invoke();
        }

    }
}

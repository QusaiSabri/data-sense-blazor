using System.Data;
using data_sense_blazor.Models;
using System.Collections.Generic;

namespace data_sense_blazor.Shared
{
    public class AppState
    {
        private List<Action> Observers = new List<Action>();
        public string ConnectionString { get; private set; } = String.Empty;
        public bool IsConnected { get; private set; } = false;
        public List<string> Databases { get; private set; } = new List<string>();
        public string Query { get; private set; } = String.Empty;
        public DataTable QueryResult { get; private set; } = new DataTable();
        public List<string> ColumnsSelected { get; private set; } = new List<string>();
        public string SelectedDatabase { get; private set; } = String.Empty;
        public Table SelectedTable { get; private set; } = new Table();

        public void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
            NotifyStateChanged();
        }

        public void SetConnectionStatus(bool status)
        {
            IsConnected = status;
            NotifyStateChanged();
        }
        public void SetDatabases(List<string> databases)
        {
            Databases = databases;
            NotifyStateChanged();
        }
        public void AddToColumnsSelected(string column)
        {
            ColumnsSelected.Add(column);
            NotifyStateChanged();
        }

        public void RemoveFromColumnsSelected(string column)
        {
            ColumnsSelected.Remove(column);
            NotifyStateChanged();
        }

        public void SetQueryResult(DataTable queryResult)
        {
            QueryResult = queryResult;
            NotifyStateChanged();
        }

        public void SetQuery(string query)
        {
            Query = query;
            NotifyStateChanged();
        }

        public void SetColumnsSelected(List<string> columnsSelected)
        {
            ColumnsSelected = columnsSelected;
            NotifyStateChanged();
        }

        public void SetSelectedTable(Table selectedTable)
        {
            SelectedTable = selectedTable;
            NotifyStateChanged();
        }

        public void SetSelectedDatabase(string selectedDatabase)
        {
            SelectedDatabase = selectedDatabase;
            NotifyStateChanged();
        }

        public void RegisterStateChangeDelegate(Action stateHasChanged)
        {
            Observers.Add(stateHasChanged);
        }

        public void UnregisterStateChangeDelegate(Action stateHasChanged)
        {
            Observers.Remove(stateHasChanged);
        }

        private void NotifyStateChanged()
        {
            foreach (var observer in Observers)
            {
                Console.WriteLine("Invoking observer: " + observer.Method.Name);
                observer.Invoke();
            }
        }

    }
}

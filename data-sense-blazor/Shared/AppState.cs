using System.Data;
using data_sense_blazor.Models;

namespace data_sense_blazor.Shared
{
    public class AppState
    {
        public string Query { get; private set; } = String.Empty;
        public DataTable QueryResult { get; private set; } = new DataTable();
        public List<string> ColumnsSelected { get; private set; } = new List<string>();
        public Table SelectedTable { get; private set; } = new Table();
        public string SelectedDatabase { get; private set; } = String.Empty;

        public event Action OnQueryResultChange;
        public event Action OnQueryChange;
        public event Action OnColumnsSelectedChange;
        public event Action OnSelectedTableChange;
        public event Action OnSelectedDatabaseChange;

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
            OnColumnsSelectedChange?.Invoke();
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

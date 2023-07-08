using System.Data;

namespace data_sense_blazor.Shared
{
    public class AppState
    {
        public string Query { get; private set; } = String.Empty;

        public DataTable QueryResult { get; private set; } = new DataTable();

        public event Action OnQueryResultChange;

        public event Action OnQueryChange;

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

    }
}

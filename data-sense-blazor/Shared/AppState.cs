using System.Data;

namespace data_sense_blazor.Shared
{
    public class AppState
    {
        public DataTable QueryResult { get; private set; } = new DataTable();

        public event Action OnQueryResultChange;

        public void SetQueryResult(DataTable queryResult)
        {
            QueryResult = queryResult;
            OnQueryResultChange?.Invoke();
        }

    }
}

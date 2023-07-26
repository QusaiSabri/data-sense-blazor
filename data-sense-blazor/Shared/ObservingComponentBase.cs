using Microsoft.AspNetCore.Components;

namespace data_sense_blazor.Shared
{
    public class ObservingComponentBase : ComponentBase, IDisposable
    {
        [Inject]
        protected AppState AppState { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            AppState.RegisterStateChangeDelegate(StateHasChanged);
        }

        public void Dispose()
        {
            AppState.UnregisterStateChangeDelegate(StateHasChanged);
        }
    }
}

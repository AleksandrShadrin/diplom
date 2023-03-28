using PSASH.Core.Entities;

namespace PSASH.Presentation.Services
{
    public class AppStateService
    {
        public Dataset Dataset { get; private set; }
        public bool ServerState { get; private set; } = false;

        public event Action OnDatasetChange;

        public void SetDataset(Dataset dataset)
        {
            Dataset = dataset;
            NotifyDatasetChanged();
        }
        private void NotifyDatasetChanged() => OnDatasetChange?.Invoke();
    }
}

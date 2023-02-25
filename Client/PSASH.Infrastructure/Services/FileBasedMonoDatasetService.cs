using PSASH.Core.Entities;

namespace PSASH.Infrastructure.Services
{
    public class FileBasedMonoDatasetService : IFileBasedMonoDatasetService
    {
        public Dataset LoadDataset()
        {
            throw new NotImplementedException();
        }

        public MonoTimeSeries LoadTimeSeries(string name)
        {
            throw new NotImplementedException();
        }

        public void SetPath(string path)
        {
            throw new NotImplementedException();
        }
    }
}
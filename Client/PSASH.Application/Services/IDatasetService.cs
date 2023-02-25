using PSASH.Core.Entities;

namespace PSASH.Application.Services
{
    public interface IDatasetService<T>
        where T : BaseTimeSeries
    {
        Dataset LoadDataset();
        T LoadTimeSeries(string name);
    }
}
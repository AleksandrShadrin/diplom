using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;

namespace PSASH.Application.Services
{
    public interface IDatasetService<T>
        where T : BaseTimeSeries
    {
        Dataset LoadDataset();
        T LoadTimeSeries(TimeSeriesInfo info);
    }
}
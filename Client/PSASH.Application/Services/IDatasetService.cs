using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;

namespace PSASH.Application.Services
{
    public interface IDatasetService<T>
        where T : BaseTimeSeries
    {
        /// <summary>
        /// Загружает датасет
        /// </summary>
        /// <returns>Возвращает Dataset</returns>
        Dataset LoadDataset();
        /// <summary>
        /// Загружает временной ряд из датасета
        /// </summary>
        /// <param name="info">информация об временном ряде</param>
        /// <returns>Возвращает временной ряд</returns>
        T LoadTimeSeries(TimeSeriesInfo info);
    }
}
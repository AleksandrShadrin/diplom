using PSASH.Core.Entities;

namespace PSASH.Infrastructure.Services
{
    public interface IMonoTimeSeriesModelTrainer
    {
        /// <summary>
        /// Обучение модели
        /// </summary>
        /// <param name="model">Необученная модель</param>
        /// <param name="datasetName">Имя датасета</param>
        /// <param name="timeSeries">Список временных рядов</param>
        /// <returns>Возвращает TrainedModel</returns>
        Task<TrainedModel> TrainModel(UntrainedModel model, string datasetName, List<MonoTimeSeries> timeSeries);
        /// <summary>
        /// Возвращает готовые к обучению модели
        /// </summary>
        /// <returns>Возвращает список UntrainedModel</returns>
        Task<List<UntrainedModel>> GetUntrainedModels();
        /// <summary>
        /// Возвращает имена загруженных наборов данных
        /// </summary>
        /// <returns>
        /// Возвращает строковый список
        /// </returns>
        Task<List<string>> GetDatasetNames();
    }
}

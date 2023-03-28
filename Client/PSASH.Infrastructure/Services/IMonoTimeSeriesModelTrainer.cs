using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;

namespace PSASH.Infrastructure.Services
{
    public interface IMonoTimeSeriesModelTrainer
    {
        /// <summary>
        /// Обучение модели
        /// </summary>
        /// <param name="model">Необученная модель</param>
        /// <param name="datasetName">Имя набора данных</param>
        /// <param name="dataset">Dataset, который необходимо отправить</param>
        /// <returns>Возвращает TrainedModel</returns>
        Task<TrainedModel> TrainModel(UntrainedModel model, string datasetName, Dataset dataset);
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
        ///<summary>
        /// Устанавливает опции для тренировки набора данных
        /// </summary>
        /// <param name="options">
        /// Параметр опций, которые необходимо установить
        /// </param>
        void SetTrainOptions(TrainOptions options);
    }
}

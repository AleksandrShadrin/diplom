using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;

namespace PSASH.Application.Services
{
    public interface ILearningService
    {
        /// <summary>
        /// обучение модели
        /// </summary>
        /// <param name="model">имя модели</param>
        /// <param name="options">параметры обучения</param>
        /// <returns>Возвращает результат обучения true или false</returns>
        Task<bool> TrainModel(UntrainedModel model, TrainOptions options);

        /// <summary>
        /// получает обученные модели
        /// </summary>
        /// <returns>возвращает список готовых моделей</returns>
        Task<List<TrainedModel>> GetTrainedModels();

        /// <summary>
        /// Возвращает существующие модели для обучения
        /// </summary>
        /// <returns>возвращает список необученных моделей</returns>
        Task<List<UntrainedModel>> GetUntrainedModels();
    }
}
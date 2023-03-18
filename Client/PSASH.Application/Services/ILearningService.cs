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
        /// <param name="options">параметры обучени¤</param>
        /// <returns>¬озвращает обученную модель</returns>
        Task<TrainedModel> TrainModel(UntrainedModel model, TrainOptions options);

        /// <summary>
        /// получает обученные модели
        /// </summary>
        /// <returns>возвращает список готовых моделей</returns>
        Task<List<TrainedModel>> GetTrainedModels();

        /// <summary>
        /// ¬озвращает существующие модели для обучения
        /// </summary>
        /// <returns>возвращает список необученных моделей</returns>
        Task<List<UntrainedModel>> GetUntrainedModels();
    }
}
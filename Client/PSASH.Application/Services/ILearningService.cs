using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;

namespace PSASH.Application.Services
{
    public interface ILearningService
    {
        /// <summary>
        /// ќбучение модели
        /// </summary>
        /// <param name="model">им€ модели</param>
        /// <param name="options">параметры обучени€</param>
        /// <returns>¬озвращает обученную модель</returns>
        TrainedModel TrainModel(UntrainedModel model, TrainOptions options);

        /// <summary>
        /// ѕолучает обученные модели
        /// </summary>
        /// <returns>¬озвращает список готовых моделей</returns>
        List<TrainedModel> GetTrainedModels();

        /// <summary>
        /// ¬озвращает существующие модели дл€ обучени€
        /// </summary>
        /// <returns>¬озвращает список необученных моделей</returns>
        List<UntrainedModel> GetUntrainedModels();
    }
}
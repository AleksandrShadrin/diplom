using PSASH.Core.Entities;

namespace PSASH.Infrastructure.Services
{
    public interface ITrainedModelsLoader
    {
        /// <summary>
        /// Загружает готовую модель по её имени
        /// </summary>
        /// <param name="modelName">имя модели</param>
        /// <returns>Возвращает TrainedModel</returns>
        TrainedModel LoadModel(string modelName);

        /// <summary>
        /// Загружает все обученные модели
        /// </summary>
        /// <returns>Возвращает список TrainedModel</returns>
        List<TrainedModel> LoadModels();
    }
}

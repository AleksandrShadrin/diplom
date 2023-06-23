using PSASH.Core.Entities;

namespace PSASH.Infrastructure.Services
{
    public interface ITrainedModelsLoader
    {
        /// <summary>
        /// Загружает готовую модель по её имени
        /// </summary>
        /// <param name="guid">id модели</param>
        /// <returns>Возвращает TrainedModel</returns>
        Task<TrainedModel> LoadModel(string guid);

        /// <summary>
        /// Загружает все обученные модели
        /// </summary>
        /// <returns>Возвращает список TrainedModel</returns>
        Task<List<TrainedModel>> LoadModels();
    }
}

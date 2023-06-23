using PSASH.Application.Services;
using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;

namespace PSASH.Infrastructure.Services
{
    public class MonoTimeSeriesLearningService : ILearningService
    {
        private readonly ITrainedModelsLoader _trainedModelsLoader;
        private readonly IMonoTimeSeriesModelTrainer _modelTrainer;

        public MonoTimeSeriesLearningService(
            ITrainedModelsLoader trainedModelsLoader,
            IMonoTimeSeriesModelTrainer modelTrainer)
        {
            _modelTrainer = modelTrainer;
            _trainedModelsLoader = trainedModelsLoader;
        }

        public async Task<List<TrainedModel>> GetTrainedModels()
            => await _trainedModelsLoader.LoadModels();

        public async Task<List<UntrainedModel>> GetUntrainedModels()
            => await _modelTrainer.GetUntrainedModels();

        public async Task<bool> TrainModel(UntrainedModel model, TrainOptions options)
        {
            _modelTrainer.SetTrainOptions(options);

            return await _modelTrainer
                .TrainModel(model);
        }
    }
}

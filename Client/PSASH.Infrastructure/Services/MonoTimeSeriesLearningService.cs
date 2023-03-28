using PSASH.Application.Services;
using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;

namespace PSASH.Infrastructure.Services
{
    public class MonoTimeSeriesLearningService : ILearningService
    {
        private readonly IDatasetService<MonoTimeSeries> _datasetService;
        private readonly ITrainedModelsLoader _trainedModelsLoader;
        private readonly IMonoTimeSeriesModelTrainer _modelTrainer;

        public MonoTimeSeriesLearningService(
            IDatasetService<MonoTimeSeries> datasetService,
            ITrainedModelsLoader trainedModelsLoader,
            IMonoTimeSeriesModelTrainer modelTrainer)
        {
            _datasetService = datasetService;
            _modelTrainer = modelTrainer;
            _trainedModelsLoader = trainedModelsLoader;
        }

        public async Task<List<TrainedModel>> GetTrainedModels()
            => await _trainedModelsLoader.LoadModels();

        public async Task<List<UntrainedModel>> GetUntrainedModels()
            => await _modelTrainer.GetUntrainedModels();

        public async Task<TrainedModel> TrainModel(UntrainedModel model, TrainOptions options)
        {
            var dataset = _datasetService.LoadDataset();
            _modelTrainer.SetTrainOptions(options);

            var result = await _modelTrainer
                .TrainModel(model,
                    dataset.Name,
                    dataset);

            return result;
        }
    }
}

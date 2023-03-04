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

        public List<TrainedModel> GetTrainedModels()
            => _trainedModelsLoader.LoadModels();

        public List<UntrainedModel> GetUntrainedModels()
            => _modelTrainer.GetUntrainedModels();

        public TrainedModel TrainModel(UntrainedModel model, TrainOptions options)
        {
            var dataset = _datasetService.LoadDataset();
            var timeSeries = dataset
                .GetValues()
                .Select(tsi => _datasetService.LoadTimeSeries(tsi));

            var result = _modelTrainer
                .TrainModel(model,
                    dataset.Name,
                    timeSeries.ToList());

            return result;
        }
    }
}

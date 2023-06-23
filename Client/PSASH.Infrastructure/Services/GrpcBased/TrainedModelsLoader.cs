using Grpc.Client.ClientForLearning;
using PSASH.Core.Entities;

namespace PSASH.Infrastructure.Services.GrpcBased
{
    public class TrainedModelsLoader : ITrainedModelsLoader
    {
        private readonly ILearningClient _learningClient;
        private List<TrainedModel> _models = new();

        public TrainedModelsLoader(ILearningClient learningClient)
        {
            _learningClient = learningClient;
        }

        public async Task<TrainedModel?> LoadModel(string guid)
        {
            var model = _models.FirstOrDefault(m => m.Id.ToString() == guid);

            if (model is null)
                model = (await LoadModels())
                    .FirstOrDefault(m => m.Id.ToString() == guid);

            return model;
        }

        public async Task<List<TrainedModel>> LoadModels()
        {
            var models = await _learningClient.GetTrainedModels();

            _models = models.Select(m => new TrainedModel(m.DatasetName,
                Guid.Parse(m.Id),
                m.ModelName,
                new Core.ValueObjects.ModelStatistics(m.Statistics)))
                .ToList();

            return _models.ToList();
        }
    }
}

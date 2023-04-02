using Grpc.Client.ClientForLearning;
using PSASH.Core.Entities;

namespace PSASH.Infrastructure.Services.GrpcBased
{
    public class TrainedModelsLoader : ITrainedModelsLoader
    {
        private readonly ILearningClient _learningClient;

        public TrainedModelsLoader(ILearningClient learningClient)
        {
            _learningClient = learningClient;
        }

        public Task<TrainedModel> LoadModel(string guid)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TrainedModel>> LoadModels()
        {
            var models = await _learningClient.GetTrainedModels();

            return models.Select(m => new TrainedModel(m.ModelName,
                Guid.Parse(m.Id),
                new Core.ValueObjects.ModelStatistics(m.Statistics)))
                .ToList();
        }
    }
}

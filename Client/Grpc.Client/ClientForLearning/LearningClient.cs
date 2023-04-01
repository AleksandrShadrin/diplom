using Grpc.Client.Models;
using Grpc.Net.Client;

namespace Grpc.Client.ClientForLearning
{
    public class LearningClient : ILearningClient
    {
        private readonly GrpcChannel _channel;

        public LearningClient(GrpcChannel channel)
        {
            _channel = channel;
        }

        public async Task<List<string>> GetModelsNamesForLearning()
        {
            var client = new Learning.LearningService.LearningServiceClient(_channel);

            var result = await client
                .GetLearningModelsNamesAsync(new Google.Protobuf.WellKnownTypes.Empty());

            return result.Names.ToList();
        }

        public async Task<List<TrainedModel>> GetTrainedModels()
        {
            var client = new Learning.LearningService.LearningServiceClient(_channel);

            var result = await client
                .GetTrainedModelsAsync(new Google.Protobuf.WellKnownTypes.Empty());

            return result
                .Models
                .Select(model => new TrainedModel(model.DatasetName,
                model.ModelName,
                model.Uuid,
                model.Statistics.ToDictionary((s) => s.Key,
                    (st) => st.Value)))
                .ToList();
        }

        public Task<TrainStatus> TrainModel(string datasetName, Model model)
        {
            throw new NotImplementedException();
        }
    }
}

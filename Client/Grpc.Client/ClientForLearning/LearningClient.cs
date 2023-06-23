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

        public async Task<TrainStatus> TrainModel(Model model)
        {
            var client = new Learning.LearningService.LearningServiceClient(_channel);

            var learningModelRequest = new Learning.Model()
            {
                DatasetName = model.DatasetName,
                ModelName = model.ModelName,
                ModelParameters = ConvertModelParameters(model.Parameters)
            };

            var result = await client.TrainModelAsync(learningModelRequest);

            return result.Status switch
            {
                Learning.TrainStatus.Error => TrainStatus.ERROR,
                Learning.TrainStatus.Success => TrainStatus.SUCCESS,
            };
        }

        private Learning.ModelParameters ConvertModelParameters(ModelParameters parameters)
        {
            var result = new Learning.ModelParameters();

            result.CutParameters = parameters.CutType switch
            {
                CutType.CUT_BOTH => Learning.CutParameters.CutBoth,
                CutType.CUT_LEFT => Learning.CutParameters.CutLeft,
                CutType.CUT_RIGHT => Learning.CutParameters.CutRight,
            };

            result.FillParameters = parameters.FillType switch
            {
                FillType.FILL_ZEROES_BOTH => Learning.FillParameters.FillZeroesBoth,
                FillType.FILL_ZEROES_LEFT => Learning.FillParameters.FillZeroesLeft,
                FillType.FILL_ZEROES_RIGHT => Learning.FillParameters.FillZeroesRight
            };

            result.TimeSeriesLength = parameters.TimeSeriesLength;

            return result;
        }
    }
}

using Google.Protobuf.Collections;
using Grpc.Client.Models;
using Grpc.Net.Client;

namespace Grpc.Client.ClientForPrediction
{
    public class PredictionClient : IPredictionClient
    {
        private readonly GrpcChannel _channel;

        public PredictionClient(GrpcChannel channel)
        {
            _channel = channel;
        }

        public async Task<string> Predict(string modelId, TimeSeriesDto timeSeriesDto)
        {
            var client = new Prediction.ModelsService.ModelsServiceClient(_channel);

            var model = new Prediction.Model();
            model.Uuid = modelId;

            var request = new Prediction.ModelPredictionRequest();

            request.Model = model;

            var values = new Prediction.TimeSeriesValues();
            values.Values.AddRange(timeSeriesDto.Values);

            request.TimeSeriesValues = values;

            return (await client.GetModelPredictionAsync(request)).ClassifiedAs;
        }
    }
}

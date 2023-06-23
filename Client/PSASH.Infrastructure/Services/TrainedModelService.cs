using Grpc.Client.ClientForPrediction;
using PSASH.Application.Services;
using PSASH.Application.ValueObjects;
using PSASH.Core.Entities;

namespace PSASH.Infrastructure.Services
{
    public class TrainedModelService : ITrainedModelService<MonoTimeSeries>
    {
        private readonly IPredictionClient _predictionClient;

        public TrainedModelService(IPredictionClient predictionClient)
        {
            _predictionClient = predictionClient;
        }

        public async Task<ModelPredictedResult> Predict(TrainedModel model, MonoTimeSeries timeSeries)
        {
            var prediction = await _predictionClient.Predict(model.Id.ToString(),
                ConvertToTimeSeriesDto(timeSeries));

            var predictedResult = new ModelPredictedResult(prediction);

            return predictedResult;
        }

        public Grpc.Client.Models.TimeSeriesDto ConvertToTimeSeriesDto(MonoTimeSeries ts)
            => new(ts.GetValues().ToList(),
                ts.TimeSeriesInfo.Id,
                ts.TimeSeriesInfo.Class);
    }
}

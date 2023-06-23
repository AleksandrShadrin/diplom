using Grpc.Client.Models;

namespace Grpc.Client.ClientForPrediction
{
    public interface IPredictionClient
    {
        Task<string> Predict(string modelId, TimeSeriesDto timeSeriesDto);
    }
}

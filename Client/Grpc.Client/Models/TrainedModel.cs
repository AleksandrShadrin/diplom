namespace Grpc.Client.Models
{
    public record TrainedModel(string DatasetName, string ModelName, string Id, Dictionary<string, double> Statistics);
}

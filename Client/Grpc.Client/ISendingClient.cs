using Grpc.Client.Models;

namespace Grpc.Client
{
    public interface ISendingClient
    {
        Task<Response> SendDataset(IEnumerable<DatasetShard> datasetShards);
        Task<List<string>> GetLoadedDatasetNames();
    }
}

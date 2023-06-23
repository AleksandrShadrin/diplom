using Grpc.Client.Models;

namespace Grpc.Client.ClientForSending
{
    public interface ISendingClient
    {
        Task<Response> SendDataset(IEnumerable<DatasetShard> datasetShards, CancellationToken token);
        Task<List<string>> GetLoadedDatasetNames();
    }
}
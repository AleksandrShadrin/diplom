using Grpc.Client.Models;
using Grpc.Core;
using Grpc.Net.Client;
using Sending;

namespace Grpc.Client
{
    public class SendingClient : ISendingClient, IDisposable
    {
        private readonly GrpcChannel _channel;

        public SendingClient(GrpcChannel channel)
        {
            _channel = channel;
        }

        public void Dispose()
        {
            _channel.Dispose();
        }

        public async Task<List<string>> GetLoadedDatasetNames()
        {
            var client = new DatasetSender.DatasetSenderClient(_channel);

            var names = await client
                .GetLoadedDatasetNamesAsync(new
                    Google.Protobuf.WellKnownTypes.Empty());

            return names.Names.ToList();
        }

        public async Task<Models.Response> SendDataset(IEnumerable<Models.DatasetShard> datasetShards)
        {
            var client = new DatasetSender.DatasetSenderClient(_channel);

            using var call = client.SendDataset();

            var datasetShardsToSend = new List<Models.DatasetShard>();

            var task = default(Task);

            foreach (var datasetShard in datasetShards)
            {
                datasetShardsToSend.Add(datasetShard);

                if (datasetShardsToSend.Count is 100)
                {
                    if (task is not null)
                        await task;

                    task = SendDatasetShards(datasetShardsToSend, call);
                    datasetShardsToSend = new();
                }
            }

            if (task is not null)
                await task;

            await SendDatasetShards(datasetShardsToSend, call);

            await call.RequestStream.CompleteAsync();

            var res = await call;

            return Models.Response.FromProtobufResponse(res);
        }

        private async Task SendDatasetShards(List<Models.DatasetShard> shards, 
            AsyncClientStreamingCall<Sending.DatasetShard,
            SendResponse> streamingCall)
        {
            foreach(var datasetShard in shards)
            {
                var dataShard = datasetShard.ToGrpcDatasetShard();
                await streamingCall.RequestStream.WriteAsync(dataShard);
            }
        }
    }
}

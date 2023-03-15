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

            foreach (var datasetShard in datasetShards)
            {
                var dataShard = datasetShard.ToGrpcDatasetShard();
                await call.RequestStream.WriteAsync(dataShard);
            }

            await call.RequestStream.CompleteAsync();

            var res = await call;

            return Models.Response.FromProtobufResponse(res);

        }
    }
}

using Grpc.Net.Client;
using Sending;

namespace Grpc.Client
{
    public class SendingClient : ISendingClient, IDisposable
    {
        private readonly GrpcChannel _channel;

        public SendingClient(string url)
        {
            _channel = GrpcChannel.ForAddress(url);
        }

        public void Dispose()
        {
            _channel.Dispose();
        }

        public async Task<Models.Response> SendDataset(IEnumerable<Models.DatasetShard> datasetShards)
        {
            var client = new DatasetSender.DatasetSenderClient(_channel);
            
            using (var call = client.SendDataset())
            {
                foreach (var datasetShard in datasetShards)
                {
                    var datashard = datasetShard.ToGrpcDatasetShard();
                    await call.RequestStream.WriteAsync(datashard);
                }

                await call.RequestStream.CompleteAsync();

                var res = await call;

                return Models.Response.FromProtobufResponse(res);
            }
        }
    }
}

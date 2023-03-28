using Grpc.Core;
using Grpc.Net.Client;

namespace Grpc.Client
{
    public class HealthClient : IHealthClient
    {
        private readonly GrpcChannel _channel;
        private readonly Health.Health.HealthClient _client;

        public HealthClient(GrpcChannel channel)
        {
            _client = new Health.Health.HealthClient(channel);
        }

        public async Task<bool> ServerIsServing()
        {
            try
            {
                var result = await _client.CheckAsync(new());

                if (result.Status == Health.ServingStatus.Serving)
                    return true;
            }
            catch (RpcException) { }

            return false;
        }
    }
}

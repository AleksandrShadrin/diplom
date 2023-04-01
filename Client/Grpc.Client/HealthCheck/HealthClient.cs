using Grpc.Core;
using Grpc.Net.Client;

namespace Grpc.Client.HealthCheck
{
    public class HealthClient : IHealthClient
    {
        private readonly GrpcChannel _channel;

        public HealthClient(GrpcChannel channel)
        {
            _channel = channel;
        }

        public async Task<bool> ServerIsServing()
        {
            var client = new Health.Health.HealthClient(_channel);
            try
            {
                var result = await client.CheckAsync(new());

                if (result.Status == Health.ServingStatus.Serving)
                    return true;
            }
            catch (RpcException) { }

            return false;
        }
    }
}

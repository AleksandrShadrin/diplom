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
                var taskWithResponse =  Task.Run(async () 
                    => await client.CheckAsync(new()));

                var taskWithTimeLimit = Task.Run(async () =>
                {
                    await Task.Delay(10000);
                    var resp = new Health.HealthCheckResponse();
                    resp.Status = Health.ServingStatus.NotServing;

                    return resp;
                });

                var completedTask = await Task.WhenAny(new[] {taskWithResponse, taskWithTimeLimit});
                var res = await completedTask;

                if (res.Status == Health.ServingStatus.Serving)
                    return true;
            }
            catch (RpcException) { }

            return false;
        }
    }
}

namespace Grpc.Client.HealthCheck
{
    public class ServerStatusService
    {
        IHealthClient _healthClient;
        public ServerStatusService(IHealthClient client)
        {
            _healthClient = client;
        }

        public async Task<bool> IsReadyAsync()
        {
            var res = await _healthClient.ServerIsServing();
            return res;
        }
    }
}

using Grpc.Client.HealthCheck;

namespace PSASH.Presentation.Services
{
    public class ServerStateCheckerService
    {
        private bool serverServing = false;
        private ServerStatusService _serverStatusService;
        private event Action<bool> _onStateChanged;

        public ServerStateCheckerService(ServerStatusService serverStatusService)
        {
            _serverStatusService = serverStatusService;
        }

        public Task StartAsync(int interval, Action<bool> onStateChanged, CancellationToken token)
        {
            CheckServerIsServing(interval, token);
            _onStateChanged += onStateChanged;
            return Task.CompletedTask;
        }

        private async Task CheckServerIsServing(int interval, CancellationToken token)
        {
            while (token.IsCancellationRequested is false)
            {
                var status = await _serverStatusService.IsReadyAsync();

                if (status != serverServing)
                {
                    _onStateChanged?.Invoke(status);
                    serverServing = status;
                }

                await Task.Delay(interval);
            }
        }
    }
}

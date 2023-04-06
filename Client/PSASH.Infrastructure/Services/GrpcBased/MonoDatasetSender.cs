using Grpc.Client.ClientForSending;
using Grpc.Client.Models;
using PSASH.Application.Services;
using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;
using PSASH.Infrastructure.Exceptions;
using PSASH.Infrastructure.Models;

namespace PSASH.Infrastructure.Services.GrpcBased
{
    public class MonoDatasetSender : IDatasetSender
    {
        private readonly IDatasetService<MonoTimeSeries> _datasetService;
        private readonly ISendingClient _sendingClient;
        private CancellationTokenSource _cancellationTokenSource = new();
        private InfrastructureException savedInfrastructureExceptions  = default;


        public MonoDatasetSender(IDatasetService<MonoTimeSeries> datasetService, ISendingClient sendingClient)
        {
            _datasetService = datasetService == null ? throw new ArgumentNullException(
                nameof(datasetService),
                "dataset service shouldn't be null.") : datasetService;

            _sendingClient = sendingClient == null ? throw new ArgumentNullException(
                nameof(sendingClient),
                "sendingClient shouldn't be null.") : sendingClient;
        }

        public async Task<Result> SendDataset(Dataset dataset, bool rewrite)
        {
            var datasetShards = dataset
                .GetValues()
                .AsParallel()
                .Select(WrapLoadTimeSeries)
                .Select(ConvertToTimeSeriesDto)
                .Select(ts => new DatasetShard(rewrite, dataset.Name, ts));

            try
            {
                _cancellationTokenSource = new();

                var res = await _sendingClient
                    .SendDataset(datasetShards, _cancellationTokenSource.Token);

                return res.Status == Status.Success ?
                    Result.Ok(res.Message) :
                    Result.Error(res.Message);
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine(ex.Message);
                throw savedInfrastructureExceptions;
            }
        }

        private MonoTimeSeries WrapLoadTimeSeries(TimeSeriesInfo info)
        {
            try
            {
                return _datasetService.LoadTimeSeries(info);
            }
            catch (InfrastructureException ex)
            {
                _cancellationTokenSource.Cancel();
                savedInfrastructureExceptions = ex;
            }

            return new MonoTimeSeries(Enumerable.Empty<double>(), info);
        }

        public async Task<List<string>> GetLoadedDatasetNames()
            => await _sendingClient.GetLoadedDatasetNames();

        public TimeSeriesDto ConvertToTimeSeriesDto(MonoTimeSeries ts)
            => new(ts.GetValues().ToList(),
                ts.TimeSeriesInfo.Id,
                ts.TimeSeriesInfo.Class);
    }
}

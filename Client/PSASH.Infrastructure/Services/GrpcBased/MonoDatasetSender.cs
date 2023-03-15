using Grpc.Client;
using Grpc.Client.Models;
using PSASH.Application.Services;
using PSASH.Core.Entities;
using PSASH.Infrastructure.Models;

namespace PSASH.Infrastructure.Services.GrpcBased
{
    public class MonoDatasetSender : IDatasetSender
    {
        private readonly IDatasetService<MonoTimeSeries> _datasetService;
        private readonly ISendingClient _sendingClient;


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
                .Select(_datasetService.LoadTimeSeries)
                .Select(ConvertToTimeSeriesDto)
                .Select(ts => new DatasetShard(rewrite, dataset.Name, ts));

            var res = await _sendingClient.SendDataset(datasetShards);

            return res.Status == Status.Success ?
                Result.Ok(res.Message) :
                Result.Error(res.Message);
        }

        public TimeSeriesDto ConvertToTimeSeriesDto(MonoTimeSeries ts)
            => new(ts.GetValues().ToList(),
                ts.TimeSeriesInfo.id,
                ts.TimeSeriesInfo.Class);
    }
}

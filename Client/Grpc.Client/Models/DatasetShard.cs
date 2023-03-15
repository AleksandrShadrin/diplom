namespace Grpc.Client.Models
{
    public record DatasetShard(bool UpdateDataset, string DatasetName, TimeSeriesDto TimeSeries)
    {
        public Sending.DatasetShard ToGrpcDatasetShard()
        {
            var datasetShard = new Sending.DatasetShard();

            datasetShard.UpdateDataset = UpdateDataset;
            datasetShard.DatasetName = DatasetName;
            datasetShard.TimeSeries = TimeSeries.ToGrpcTimeSeries();

            return datasetShard;
        }
    }
}

namespace Grpc.Client.Models
{
    public record TimeSeriesDto(List<double> Values, string Id, string ClassName)
    {
        internal Sending.TimeSeries ToGrpcTimeSeries()
        {
            var sendingTimeSeries = new Sending.TimeSeries();

            sendingTimeSeries.Id = Id;
            sendingTimeSeries.Values.AddRange(Values);
            sendingTimeSeries.ClassName = ClassName;

            return sendingTimeSeries;
        }
    }
}

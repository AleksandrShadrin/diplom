using PSASH.Core.Entities;

namespace PSASH.Presentation.Models
{
    public record TimeSeriesDto(List<double> Values, TimeSeriesName Name)
    {
        public static implicit operator TimeSeriesDto(MonoTimeSeries timeSeries)
            => new TimeSeriesDto(timeSeries.GetValues().ToList(),
                TimeSeriesName.FromTimeSeriesInfo(timeSeries.TimeSeriesInfo));
    }
}

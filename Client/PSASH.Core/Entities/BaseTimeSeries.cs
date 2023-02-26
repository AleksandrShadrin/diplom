using PSASH.Core.ValueObjects;

namespace PSASH.Core.Entities
{
    public abstract class BaseTimeSeries
    {
        public TimeSeriesInfo TimeSeriesInfo { get; init; }

        public BaseTimeSeries(TimeSeriesInfo info)
        {
            TimeSeriesInfo = info;
        }
    }
}
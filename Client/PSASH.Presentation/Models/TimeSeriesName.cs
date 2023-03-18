using PSASH.Core.ValueObjects;

namespace PSASH.Presentation.Models
{
    public class TimeSeriesName
    {
        public string Value { get; set; }

        private TimeSeriesName(string value)
        {
            Value = value;
        }

        public static TimeSeriesName FromTimeSeriesInfo(TimeSeriesInfo timeSeriesInfo)
            => new($"{timeSeriesInfo.Class}:{timeSeriesInfo.id}");

        public TimeSeriesInfo ToTimeSeriesInfo()
        {
            var parts = Value.Split(':');
            return new TimeSeriesInfo(parts[0], parts[1]);
        }

        public override string ToString()
            => Value;
    }
}

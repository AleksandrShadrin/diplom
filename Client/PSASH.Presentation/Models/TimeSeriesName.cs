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

        public static TimeSeriesName FromRouteString(string str)
            => new(str.Replace(",", "."));

        public string ToRouteString()
            => new(Value.Replace(".", "."));

        public TimeSeriesInfo ToTimeSeriesInfo()
        {
            var parts = Value.Split(':');
            return new TimeSeriesInfo(parts[0], parts[1]);
        }

        public override string ToString()
            => Value;
    }
}

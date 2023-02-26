using PSASH.Core.ValueObjects;

namespace PSASH.Core.Entities
{
    public class MonoTimeSeries : BaseTimeSeries
    {
        private readonly List<double> _values = new List<double>();

        public MonoTimeSeries(IEnumerable<double> values, TimeSeriesInfo info)
            : base(info)
        {
            _values.AddRange(values);
        }

        public IEnumerable<double> GetValues()
        {
            return _values;
        }
    }
}
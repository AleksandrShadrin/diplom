using PSASH.Core.ValueObjects;

namespace PSASH.Core.Entities
{
    public class PolyTimeSeries : BaseTimeSeries
    {
        private readonly List<MonoTimeSeries> _values = new List<MonoTimeSeries>();

        public PolyTimeSeries(IEnumerable<MonoTimeSeries> values, TimeSeriesInfo info)
            : base(info)
        {
            _values.AddRange(values);
        }

        public IEnumerable<MonoTimeSeries> GetValues()
        {
            return _values;
        }
    }
}
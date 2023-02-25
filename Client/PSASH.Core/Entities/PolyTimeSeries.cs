namespace PSASH.Core.Entities
{
    public class PolyTimeSeries : BaseTimeSeries
    {
        private readonly List<MonoTimeSeries> _values = new List<MonoTimeSeries>();

        public PolyTimeSeries(IEnumerable<MonoTimeSeries> values, string name)
            : base(name)
        {
            _values.AddRange(values);
        }

        public IEnumerable<MonoTimeSeries> GetValues()
        {
            return _values;
        }
    }
}
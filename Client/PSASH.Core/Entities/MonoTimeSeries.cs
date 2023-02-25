namespace PSASH.Core.Entities
{
    public class MonoTimeSeries : BaseTimeSeries
    {
        private readonly List<double> _values = new List<double>();

        public MonoTimeSeries(IEnumerable<double> values, string name)
            : base(name)
        {
            _values.AddRange(values);
        }

        public IEnumerable<double> GetValues()
        {
            return _values;
        }
    }
}
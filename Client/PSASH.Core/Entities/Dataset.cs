using PSASH.Core.ValueObjects;

namespace PSASH.Core.Entities
{
    public class Dataset
    {
        public string Name { get; init; } = String.Empty;
        private readonly List<TimeSeriesInfo> _data = new List<TimeSeriesInfo>();

        public Dataset(IEnumerable<TimeSeriesInfo> data, string name)
        {
            _data.AddRange(data);
            Name = name;
        }

        public IEnumerable<TimeSeriesInfo> GetValues()
            => _data;
    }
}
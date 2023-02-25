namespace PSASH.Core.Entities
{
    public class Dataset
    {
        public string Name { get; init; } = String.Empty;
        private readonly List<string> _data = new List<string>();

        public Dataset(IEnumerable<string> data, string name)
        {
            _data.AddRange(data);
            Name = name;
        }

        public IEnumerable<string> GetValues()
            => _data;
    }
}
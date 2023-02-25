namespace PSASH.Core.Entities
{
    public abstract class BaseTimeSeries
    {
        public string Name { get; init; } = string.Empty;

        public BaseTimeSeries(string name)
        {
            Name = name;
        }
    }
}
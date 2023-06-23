namespace PSASH.Core.ValueObjects
{
    public class ModelStatistics
    {
        public Dictionary<string, string> Stats { get; init; } = null!;

        public ModelStatistics(Dictionary<string, string> stats)
        {
            if (stats is null)
                throw new ArgumentNullException(
                    nameof(stats),
                    "Stats can't be null");

            Stats = stats;
        }

    }
}

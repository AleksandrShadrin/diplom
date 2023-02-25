namespace PSASH.Core.ValueObjects
{
    public class ModelStatistics
    {
        public Dictionary<string, double> Stats { get; init; } = null!;

        public ModelStatistics(Dictionary<string, double> stats)
        {
            if (stats is null)
                throw new ArgumentNullException(
                    nameof(stats),
                    "Stats can't be null");

            Stats = stats;
        }

    }
}

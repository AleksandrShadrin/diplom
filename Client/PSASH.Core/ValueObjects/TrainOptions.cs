using PSASH.Core.Constants;

namespace PSASH.Core.ValueObjects
{
    public record TrainOptions
    {
        public int TimeSeriesLength { get; init; }
        public Shift ShiftTo { get; init; } = Shift.RIGHT;

        public TrainOptions(int timeSeriesLength, Shift shiftTo)
        {
            if (timeSeriesLength < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(timeSeriesLength),
                    "TimeSeries length can't be < 0.");
            }

            ShiftTo = shiftTo;
        }
    }
}
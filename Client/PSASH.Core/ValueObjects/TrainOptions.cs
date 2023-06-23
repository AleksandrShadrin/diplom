using PSASH.Core.Constants;

namespace PSASH.Core.ValueObjects
{
    public record TrainOptions
    {
        public int TimeSeriesLength { get; init; }
        public CutOption CutOption { get; init; } = CutOption.CUT_RIGHT;
        public FillOption FillOption { get; set; } = FillOption.FILL_LEFT;

        public TrainOptions(int timeSeriesLength, CutOption cutOption, FillOption fillOption)
        {
            if (timeSeriesLength < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(timeSeriesLength),
                    "TimeSeries length can't be < 0.");
            }

            TimeSeriesLength = timeSeriesLength;
            CutOption = cutOption;
            FillOption = fillOption;
        }
    }
}
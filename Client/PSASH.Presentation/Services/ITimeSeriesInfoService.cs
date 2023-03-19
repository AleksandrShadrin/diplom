using PSASH.Presentation.Models;

namespace PSASH.Presentation.Services
{
    public interface ITimeSeriesInfoService
    {
        double GetMean(TimeSeriesDto timeSeries);
        double GetStd(TimeSeriesDto timeSeries);

        double GetCV(TimeSeriesDto timeSeries);
        double GetMax(TimeSeriesDto timeSeries);
        double GetMin(TimeSeriesDto timeSeries);
    }
}

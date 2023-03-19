using PSASH.Presentation.Models;


namespace PSASH.Presentation.Services
{
    public class TimeSeriesInfoService : ITimeSeriesInfoService
    {
        public double GetMean(TimeSeriesDto timeSeries)
        {
            var values = timeSeries.Values;
            double sum = 0;
            foreach (var value in values)
            {
                sum += value;
            }
            return sum / values.Count;
        }
        public double GetStd(TimeSeriesDto timeSeries)
        {
            var values = timeSeries.Values;
            double sum = 0;
            foreach (var value in values)
            {
                sum += value;
            }
            double mean = sum / values.Count;
            double sumStd = 0;
            foreach (var value in values)
            {
                sumStd += Math.Pow((value - mean), 2);
            }
            return Math.Sqrt(sumStd / (values.Count-1));
        }

        public double GetCV(TimeSeriesDto timeSeries)//коэффициент вариации
        {
            return GetStd(timeSeries)*100/GetMean(timeSeries);
        }

        public double GetMax(TimeSeriesDto timeSeries)
        {
            var values = timeSeries.Values;
            return values.Max();
        }

        public double GetMin(TimeSeriesDto timeSeries)
        {
            var values = timeSeries.Values;
            return values.Min();
        }
    }
}

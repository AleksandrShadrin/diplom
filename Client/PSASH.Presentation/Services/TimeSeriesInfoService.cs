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
            return Math.Sqrt(sumStd / values.Count);
        }

        public double GetCV(TimeSeriesDto timeSeries)//коэффициент вариации
        {
            var values = timeSeries.Values;
            double sum = 0;
            foreach (var value in values)
            {
                sum += value;
            }
            double mean = sum / values.Count;
            double sum2 = 0;
            foreach (var value in values)
            {
                sum2 += Math.Pow((value - mean), 2) / (values.Count-1);
            }
            double std = Math.Sqrt(sum2);
            return std*100/mean;
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

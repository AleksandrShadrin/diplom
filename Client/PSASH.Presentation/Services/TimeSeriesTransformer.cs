using PSASH.Presentation.Models;

namespace PSASH.Presentation.Services
{
    public class TimeSeriesTransformer : ITimeSeriesTransformer
    {
        public IEnumerable<double> AverageMoving(List<double> values, int k)
        {
            var queue = new Queue<double>(k);

            foreach (double d in values)
            {
                if (queue.Count == k)
                {
                    queue.Dequeue();
                }
                queue.Enqueue(d);
                yield return queue.Average();
            }
        }

        public IEnumerable<double> FastFourierTransform(List<double> values)
        {
            var array = values.ToArray();

            return FftSharp
                .Transform
                .FFTmagnitude(FftSharp.Pad.ZeroPad(array))
                .Take(array.Length);
        }
    }
}

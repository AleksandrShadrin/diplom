using PSASH.Presentation.Models;

namespace PSASH.Presentation.Services
{
    public interface ITimeSeriesTransformer
    {
        IEnumerable<double> FastFourierTransform(List<double> values);
        IEnumerable<double> AverageMoving(List<double> values, int k);
    }
}

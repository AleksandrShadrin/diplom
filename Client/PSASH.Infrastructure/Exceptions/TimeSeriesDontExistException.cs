using PSASH.Core.ValueObjects;

namespace PSASH.Infrastructure.Exceptions
{
    public class TimeSeriesDontExistException : InfrastructureException
    {
        public TimeSeriesDontExistException(TimeSeriesInfo info)
            : base($"TimeSereis with class: {info.Class} and id: {info.Id} don't exist")
        {
        }
    }
}

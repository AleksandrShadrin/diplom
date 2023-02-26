using PSASH.Core.Entities;

namespace PSASH.Infrastructure.Services.FileBased.Converter
{
    public interface ITimeSeriesConverter<TIn, TOut>
        where TOut : BaseTimeSeries
    {
        TOut Convert(TIn input);
    }
}
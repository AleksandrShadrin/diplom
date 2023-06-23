using PSASH.Core.Entities;

namespace PSASH.Infrastructure.Services.FileBased.Converter
{
    public interface IFileBasedMonoTimeSeriesConverter :
        ITimeSeriesConverter<string, MonoTimeSeries>
    {
    }
}

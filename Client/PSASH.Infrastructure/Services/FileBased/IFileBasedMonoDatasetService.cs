using PSASH.Application.Services;
using PSASH.Core.Entities;

namespace PSASH.Infrastructure.Services.FileBased
{
    public interface IFileBasedMonoDatasetService : IDatasetService<MonoTimeSeries>, IFileBased
    { }
}

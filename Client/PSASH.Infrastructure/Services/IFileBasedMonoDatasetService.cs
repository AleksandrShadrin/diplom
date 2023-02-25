using PSASH.Application.Services;
using PSASH.Core.Entities;

namespace PSASH.Infrastructure.Services
{
    public interface IFileBasedMonoDatasetService : IDatasetService<MonoTimeSeries>, IFileBased
    { }
}

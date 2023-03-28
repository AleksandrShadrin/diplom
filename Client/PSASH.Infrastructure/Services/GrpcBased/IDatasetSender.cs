using PSASH.Core.Entities;
using PSASH.Infrastructure.Models;

namespace PSASH.Infrastructure.Services.GrpcBased
{
    public interface IDatasetSender
    {
        Task<Result> SendDataset(Dataset dataset, bool rewrite);
        Task<List<string>> GetLoadedDatasetNames();
    }
}

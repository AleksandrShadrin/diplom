using Grpc.Client.Models;

namespace Grpc.Client.ClientForLearning
{
    public interface ILearningClient
    {
        Task<List<string>> GetModelsNamesForLearning();
        Task<TrainStatus> TrainModel(Model model);
        Task<List<TrainedModel>> GetTrainedModels();

    }
}

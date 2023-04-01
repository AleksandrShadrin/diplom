using Grpc.Client.Models;

namespace Grpc.Client.ClientForLearning
{
    public interface ILearningClient
    {
        Task<List<string>> GetModelsNamesForLearning();
        Task<TrainStatus> TrainModel(string datasetName, Model model);
        Task<List<TrainedModel>> GetTrainedModels();

    }
}

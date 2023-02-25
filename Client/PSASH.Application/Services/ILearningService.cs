using PSASH.Core.Entities;
using PSASH.Core.ValueObjects;

namespace PSASH.Application.Services
{
    public interface ILearningService
    {
        TrainedModel TrainModel(UntrainedModel model, TrainOptions options);
        List<TrainedModel> GetTrainedModels();
    }
}
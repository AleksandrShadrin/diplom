using PSASH.Application.ValueObjects;
using PSASH.Core.Entities;

namespace PSASH.Application.Services
{
    public interface ITrainedModelService<T>
        where T : BaseTimeSeries
    {
        Task<ModelPredictedResult> Predict(TrainedModel model, T timeSeries);
    }
}
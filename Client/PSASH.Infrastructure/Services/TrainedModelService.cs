using PSASH.Application.Services;
using PSASH.Application.ValueObjects;
using PSASH.Core.Entities;

namespace PSASH.Infrastructure.Services
{
    public class TrainedModelService : ITrainedModelService<MonoTimeSeries>
    {
        public ModelPredictedResult Predict(TrainedModel model, MonoTimeSeries timeSeries)
        {
            throw new NotImplementedException();
        }
    }
}

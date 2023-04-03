using PSASH.Core.ValueObjects;

namespace PSASH.Core.Entities
{
    public class TrainedModel
    {
        public string DatasetName { get; init; } = String.Empty;
        public ModelStatistics ModelStatistics { get; init; }
        public string ModelName { get; init; }
        public Guid Id { get; init; }

        public TrainedModel(string datasetName, Guid id, string modelName, ModelStatistics modelStatistics)
        {
            Id = id;
            DatasetName = datasetName;
            ModelName = modelName;
            ModelStatistics = modelStatistics;
        }
    }
}
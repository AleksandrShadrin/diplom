using PSASH.Core.ValueObjects;

namespace PSASH.Core.Entities
{
    public class TrainedModel
    {
        public string DatasetName { get; init; } = String.Empty;
        public ModelStatistics ModelStatistics { get; init; }
        public Guid Id { get; init; }

        public TrainedModel(string name, Guid id, ModelStatistics modelStatistics)
        {
            Id = id;
            DatasetName = name;
            ModelStatistics = modelStatistics;
        }
    }
}
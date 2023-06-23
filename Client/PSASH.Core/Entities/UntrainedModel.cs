namespace PSASH.Core.Entities
{
    public class UntrainedModel
    {
        public string Name { get; init; } = String.Empty;
        public string DatasetName { get; set; } = string.Empty;

        public UntrainedModel(string name, string datasetName)
        {
            Name = name;
            DatasetName = datasetName;
        }
    }
}
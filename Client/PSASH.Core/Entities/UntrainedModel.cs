namespace PSASH.Core.Entities
{
    public class UntrainedModel
    {
        public string Name { get; init; } = String.Empty;

        public UntrainedModel(string name)
        {
            Name = name;
        }
    }
}